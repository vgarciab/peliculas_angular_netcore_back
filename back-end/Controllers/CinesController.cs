using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/cines")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class CinesController: ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public CinesController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet] // responderá a la URL: 'api/cines'
        public async Task<ActionResult<List<CineDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Cines.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var cines = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();

            // Ahora se trata de mapear generos hacia generosDTO:
            // Se puede hacer un mapeo más eficiente con la librería ****AutoMapper**** (mapeo objeto a objeto)
            return mapper.Map<List<CineDTO>>(cines);
        }

        [HttpGet("{Id:int}")]  // Id:int, es una restricción de variable de ruta, dándole un tipo explícitamente, un entereo, en este caso.
        public async Task<ActionResult<CineDTO>> Get(int Id) //  [BindRequired, FromHeader] string nombre
        {
            var cine = await context.Cines.FirstOrDefaultAsync(x => x.Id == Id);

            if (cine == null)
            {
                return NotFound();
            }

            return mapper.Map<CineDTO>(cine);
        }




        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineCreacionDTO cineCreacionDTO)
        {
            var cine = mapper.Map<Cine>(cineCreacionDTO);
            context.Add(cine);
            await context.SaveChangesAsync();
            return NoContent();  // retornamos un 204
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] CineCreacionDTO cineCreacionDTO)
        {
            var cine = await context.Cines.FirstOrDefaultAsync(x => x.Id == Id);

            if (cine == null)
            {
                return NotFound();
            }

            cine = mapper.Map(cineCreacionDTO, cine);

            await context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Cines.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Cine() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }




    }
}
