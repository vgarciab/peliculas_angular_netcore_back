using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
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

        public ActoresController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        // En vez de utilizar en el parámetro [FromBody], se usará [FromForm] porque 
        // vamos a poder enviar, entre otras cosas, la foto del actor.
        [HttpPost]
                public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            //var actor = mapper.Map<Actor>(actorCreacionDTO);
            //context.Add(actor);
            //await context.SaveChangesAsync();
            return NoContent();  // retornamos un 204
        }


    }
}
