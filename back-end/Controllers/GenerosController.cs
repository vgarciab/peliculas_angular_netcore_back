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
    [Route("api/generos")] // > La ruta del endpoint (Por convención, la ruta de los endpoints comienzan con la 'api/')
    public class GenerosController: ControllerBase
    {
        private readonly IRepositorio repositorio;

        public GenerosController(IRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        // Acción(es) que va a responder cuando se le haga una petición http al endpoint, el configurado en Route(..)
        
        [HttpGet]
        public List<Genero> Get()
        {
            return repositorio.ObtenerTodosLosGeneros();
        }

        [HttpGet]
        public Genero Get(int Id)
        {
            var genero = repositorio.ObtenerPorId(Id);

            if (genero == null)
            {
                // return NotFound();
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
