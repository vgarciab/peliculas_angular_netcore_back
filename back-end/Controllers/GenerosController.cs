using back_end.Entidades;
using back_end.Filtros;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using back_end.DTOs;
using AutoMapper;
using back_end.Utilidades;

namespace back_end.Controllers
{
    // También se puede escribir el endpoint como: Route("api/[controller]")
    // Si, como en este endpoint, hay dos acciones con el mismo nombre, Get() en este caso, se deben emplear las 
    // reglas de ruteo (en la Web Api, ruteo por atributo) que nos permiten mapear una URL con una acción.

    [Route("api/generos")] // > La ruta del endpoint (Por convención, la ruta de los endpoints comienzan con la 'api/')
    [ApiController] // con este atributo, se controlan las reglas de validación (devuelve los errores a quien hizo Request) de una manera ¡transparente!
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Filtro a nivel de controlador (sin autorización, no permite acciones)
    public class GenerosController: ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(ILogger<GenerosController> logger, AplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        // Acción(es) que va a responder cuando se le haga una petición http al endpoint, el configurado en Route(..)

        // Podemos tener varias anotaciones por acción
        [HttpGet] // responderá a la URL: 'api/generos'
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var generos =await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();

            // Ahora se trata de mapear generos hacia generosDTO:
            // Se puede hacer un mapeo más eficiente con la librería ****AutoMapper**** (mapeo objeto a objeto)
            return mapper.Map<List<GeneroDTO>>(generos);


            /*
               O se puede hacer el *****mapeo manual***** >>>
                   (el problema de este sistema es que si en la Class Genero se añade otra propiedad, habrá que hacer los mismo
                   en la Class GeneroDTO y, ADEMÁS, en todo el código dónde se haga:
                                '.Add(new GeneroDTO() { Id = genero.Id, Nombre = genero.Nombre, NuevaPropiedad = genero.NuevaPropiedad });',
                   siendo preciso añadir esa nueva propiedad (en el ejemplo, 'NuevaPropiedad' al mapear manualmente el objeto. Es propenso a errores.
                   

                    var resultado = new List<GeneroDTO>();
                    foreach (var genero in generos)
                    {
                        resultado.Add(new GeneroDTO() { Id = genero.Id, Nombre = genero.Nombre });

                    }
                    return resultado;

            */


        }



        //>> regla de ruteo; la Web Api responderá a la llamada con 'api/generos/1' (1 ó el Id que le pasemos);
        // {id} indica que estamos configurando una variable en la URL. 
        [HttpGet("{Id:int}")]  // Id:int, es una restricción de variable de ruta, dándole un tipo explícitamente, un entereo, en este caso.
        public async Task<ActionResult<GeneroDTO>> Get(int Id) //  [BindRequired, FromHeader] string nombre
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);

            if (genero == null)
            {
                return NotFound();
            }

            return mapper.Map<GeneroDTO>(genero);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {   
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(genero);
            await context.SaveChangesAsync();
            return NoContent();  // retornamos un 204
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);

            if (genero == null)
            {
                return NotFound();
            }

            genero = mapper.Map(generoCreacionDTO, genero);

            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Genero() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }

}
