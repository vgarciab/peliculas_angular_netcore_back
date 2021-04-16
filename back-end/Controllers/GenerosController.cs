using back_end.Entidades;
using back_end.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    // También se puede escribir el endpoint como: Route("api/[controller]")
    // Si, como en este endpoint, hay dos acciones con el mismo nombre, Get() en este caso, se deben emplear las 
    // reglas de ruteo (en la Web Api, ruteo por atributo: [ApiController]), que nos permiten mapear una URL con una acción.

    [Route("api/generos")] // > La ruta del endpoint (Por convención, la ruta de los endpoints comienzan con la 'api/')
    public class GenerosController: ControllerBase
    {
        private readonly IRepositorio repositorio;

        public GenerosController(IRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        // Acción(es) que va a responder cuando se le haga una petición http al endpoint, el configurado en Route(..)


        // Podemos tener varias anotaciones por acción
        [HttpGet] // responderá a la URL 'api/generos'
        [HttpGet("listado")] // >> regla de ruteo, que responderá a la URL 'api/generos/listado'
        [HttpGet("/listadogeneros")] // >> regla de ruteo, que responderá a la URL 'https://localhost:44315/listadogeneros' (debido al / inicial, que no hace falta todo el Route)
        public List<Genero> Get()
        {
            return repositorio.ObtenerTodosLosGeneros();
        }


        //>> regla de ruteo; la Web Api responderá a la llamada con 'api/generos/1' (1 ó el Id que le pasemos);
        // {id} indica que estamos configurando una variable en la URL. 
        [HttpGet("{Id:int}")]  // Id:int, es una restricción de variable de ruta, dándole un tipo explícitamente, un entereo, en este caso.
        public Genero Get(int Id)
        {
            var genero = repositorio.ObtenerPorId(Id);

            if (genero == null)
            {
                // return NotFound(); // para utilizar esta llamada, hay que heredar de la Class 'ControllerBase'
            }

            return genero;
        }


        [HttpPost]
        public void Post()
        {

        }


        [HttpPut]
        public void Put()
        {

        }


        [HttpDelete]
        public void Delete()
        {

        }

    }
}
