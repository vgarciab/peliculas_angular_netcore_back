using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
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


        // En vez de utilizar en el parámetro [FromBody], se usará [FromForm] porque 
        // vamos a poder enviar, entre otras cosas, la foto del actor.
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);

            /*
            // INI >> Para almacenar la foto en **Azure storage**
            if (actorCreacionDTO.Foto != null)
            {
                // GuardarArchivo(..) devuelve un string, que lo asignamos a .Foto
                actor.Foto =   
                    await this.almacenadorArchivos.GuardarArchivo(this.contenedor, actorCreacionDTO.Foto);

            }
            // << FIN  Para almacenar la foto en Azure storage
            */

            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();  // retornamos un 204
        }


    }
}
