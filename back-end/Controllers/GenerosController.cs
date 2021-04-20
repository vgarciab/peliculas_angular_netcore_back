using back_end.Entidades;
using back_end.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    // También se puede escribir el endpoint como: Route("api/[controller]")
    // Si, como en este endpoint, hay dos acciones con el mismo nombre, Get() en este caso, se deben emplear las 
    // reglas de ruteo (en la Web Api, ruteo por atributo) que nos permiten mapear una URL con una acción.

    [Route("api/generos")] // > La ruta del endpoint (Por convención, la ruta de los endpoints comienzan con la 'api/')
    [ApiController] // con este atributo, se controlan las reglas de validación (devuelve los errores a quien hizo Request) de una manera ¡transparente!
    public class GenerosController: ControllerBase
    {
        private readonly IRepositorio repositorio;
        private readonly WeatherForecastController weatherForecastController;
        private readonly ILogger<GenerosController> logger;

        public GenerosController(IRepositorio repositorio, 
                                WeatherForecastController weatherForecastController,
                                ILogger<GenerosController> logger)
        {
            this.repositorio = repositorio;
            this.weatherForecastController = weatherForecastController;
            this.logger = logger;
        }

        // Acción(es) que va a responder cuando se le haga una petición http al endpoint, el configurado en Route(..)


        // Podemos tener varias anotaciones por acción
        [HttpGet] // responderá a la URL 'api/generos'
        [HttpGet("listado")] // >> regla de ruteo, que responderá a la URL 'api/generos/listado'
        [HttpGet("/listadogeneros")] // >> regla de ruteo, que responderá a la URL 'https://localhost:44315/listadogeneros' (debido al / inicial, que no hace falta todo el Route)
        public ActionResult<List<Genero>> Get()
        {
            logger.LogInformation("Vamos a mostrar los géneros");
            return repositorio.ObtenerTodosLosGeneros();
        }


        [HttpGet("guid")] // api/generos/guid
        public ActionResult<Guid> GetGUID()
        {
            // return repositorio.ObtenerGUID();
            return Ok(new { GUID_GenerosController = repositorio.ObtenerGUID(),
                            GUID_WeatherForecastController = weatherForecastController.ObtenerGUIDWeatherForecastController()
            });

        }



        //>> regla de ruteo; la Web Api responderá a la llamada con 'api/generos/1' (1 ó el Id que le pasemos);
        // {id} indica que estamos configurando una variable en la URL. 
        [HttpGet("{Id:int}")]  // Id:int, es una restricción de variable de ruta, dándole un tipo explícitamente, un entereo, en este caso.
        public async Task<ActionResult<Genero>> Get(int Id, [FromHeader] string nombre) //  [BindRequired, FromHeader] string nombre
        {
            /* Esta comprobación (que se tendría que repetir en todos los métodos del endpoint) ya no es necesaria cuando se incluye [ApiController] >>
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // >> Retorna a la petición un 'error 400' para indicar al usuario qué reglas de validación no ha cumplido
            }
            */

            logger.LogDebug($"Obteniendo unn género por el Id {Id}");

            var genero = await repositorio.ObtenerPorId(Id);

            if (genero == null)
            {
                logger.LogWarning($"No se pudo encontrar el género con Id {Id}");
                return NotFound(); // para utilizar esta llamada, hay que heredar de la Class 'ControllerBase'
            }

            return genero;
        }


        [HttpPost]
        public ActionResult Post([FromBody] Genero genero)
        {
            repositorio.CrearGenero(genero);
            return NoContent();
        }


        [HttpPut]
        public ActionResult Put([FromBody] Genero genero)
        {
            return NoContent();
        }


        [HttpDelete]
        public void Delete()
        {

        }

    }
}
