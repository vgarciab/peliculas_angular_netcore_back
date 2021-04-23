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



        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Actores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }


    }
}
