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
    [Route("api/cines")]
    [ApiController]
    public class CinesController: ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public CinesController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineCreacionDTO cineCreacionDTO)
        {
            var cine = mapper.Map<Cine>(cineCreacionDTO);
            context.Add(cine);
            await context.SaveChangesAsync();
            return NoContent();  // retornamos un 204
        }



    }
}
