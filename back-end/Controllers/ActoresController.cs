using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/actores")] 
    [ApiController] 
    public class ActoresController: ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores"; // Para Azure Storage

        public ActoresController(AplicationDbContext context, 
                                IMapper mapper,
                                IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }


        [HttpGet] // responderá a la URL: 'api/actores'
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var actores = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();

            // Ahora se trata de mapear actores hacia ActorDTO:
            // Se puede hacer un mapeo más eficiente con la librería ****AutoMapper**** (mapeo objeto a objeto)
            return mapper.Map<List<ActorDTO>>(actores);
        }



        [HttpGet("{id:int}")]  // Id:int, es una restricción de variable de ruta, dándole un tipo explícitamente, un entereo, en este caso.
        public async Task<ActionResult<ActorDTO>> Get(int id) //  [BindRequired, FromHeader] string nombre
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return mapper.Map<ActorDTO>(actor);
        }


        // En vez de utilizar en el parámetro [FromBody], se usará [FromForm] porque 
        // vamos a poder enviar, entre otras cosas, la foto del actor.
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);

            // Para guardar una imagen (foto) localmente o en en **Azure storage**
            // (al estar inyectado en el contructor a través de almacenadorArchivos, llamará al que se haya definido en Startup.cs:
            // services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();  ó services.AddTransient<IAlmacenadorArchivos, AlmacenadorAzureStorage>();
            if (actorCreacionDTO.Foto != null)
            {
                // GuardarArchivo(..) devuelve un string, que lo asignamos a .Foto (este string es el que se almacenará en la BDD)
                actor.Foto = await almacenadorArchivos.GuardarArchivo(contenedor, actorCreacionDTO.Foto);
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();  // retornamos un 204
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            actor = mapper.Map(actorCreacionDTO, actor);


            if (actorCreacionDTO.Foto != null)
            {
                // GuardarArchivo(..) devuelve un string, que lo asignamos a .Foto (este string es el que se almacenará en la BDD)
                actor.Foto = await almacenadorArchivos.EditarArchivo(contenedor, actorCreacionDTO.Foto, actor.Foto);
            }


            await context.SaveChangesAsync();
            return NoContent();
        }






        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();

            await almacenadorArchivos.BorrarArchivo(actor.Foto, contenedor);

            return NoContent();
        }


    }
}
