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

        public GenerosController(ILogger<GenerosController> logger)
        {
            this.logger = logger;
        }

        // Acción(es) que va a responder cuando se le haga una petición http al endpoint, el configurado en Route(..)

        // Podemos tener varias anotaciones por acción
        [HttpGet] // responderá a la URL 'api/generos'
        public ActionResult<List<Genero>> Get()
        {
            return new List<Genero>()
            {
                new Genero(){Id = 1, Nombre = "Comedia" },
                new Genero(){Id = 2, Nombre = "Acción" }
            };
        }



        //>> regla de ruteo; la Web Api responderá a la llamada con 'api/generos/1' (1 ó el Id que le pasemos);
        // {id} indica que estamos configurando una variable en la URL. 
        [HttpGet("{Id:int}")]  // Id:int, es una restricción de variable de ruta, dándole un tipo explícitamente, un entereo, en este caso.
        public async Task<ActionResult<Genero>> Get(int Id) //  [BindRequired, FromHeader] string nombre
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        public ActionResult Post([FromBody] Genero genero)
        {
            throw new NotImplementedException();
        }


        [HttpPut]
        public ActionResult Put([FromBody] Genero genero)
        {
            throw new NotImplementedException();
        }


        [HttpDelete]
        public void Delete()
        {
            throw new NotImplementedException();
        }

    }
}
