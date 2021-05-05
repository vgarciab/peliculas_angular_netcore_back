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
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController: ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas"; // Para Azure Storage

        public PeliculasController(AplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            // Necesitamos IAlmacenadorArchivos porque las películas van a tener un poster, que será una imagen.
            this.almacenadorArchivos = almacenadorArchivos;
        }


        [HttpGet("PostGet")] // endpoint responderá a la URL: 'api/postget'
        public async Task<ActionResult<PeliculasPostGetDTO>> PostGet()
        {
            var cines = await context.Cines.ToListAsync();
            var generos = await context.Generos.ToListAsync();

            var cinesDTO = mapper.Map<List<CineDTO>>(cines);
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);

            return new PeliculasPostGetDTO()
            {
                Cines = cinesDTO,
                Generos = generosDTO
            };
        }



        [HttpGet]
        public async Task<ActionResult<LandingPageDTO>> Get()
        {
            var top = 6;
            var hoy = DateTime.Today;

            var proximosEstrenos = await context.Peliculas
                .Where(x => x.FechaLanzamiento > hoy)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();

            var enCines = await context.Peliculas
                .Where(x => x.EnCines)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();

            var resultado = new LandingPageDTO();
            resultado.ProximosEstrenos = mapper.Map<List<PeliculaDTO>>(proximosEstrenos);
            resultado.EnCines = mapper.Map<List<PeliculaDTO>>(enCines);

            return resultado;

        }



        [HttpGet("{id:int}")]  // Id:int, es una restricción de variable de ruta, dándole un tipo explícitamente, un entereo, en este caso.
        public async Task<ActionResult<PeliculaDTO>> Get(int id) 
        {
            var pelicula = await context.Peliculas
                    .Include(x => x.PeliculasGeneros).ThenInclude(x => x.Genero)
                    .Include(x => x.PeliculasActores).ThenInclude(x => x.Actor)
                    .Include(x => x.PeliculasCines).ThenInclude(x => x.Cine)
                    .FirstOrDefaultAsync(x => x.Id == id);


            if (pelicula == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<PeliculaDTO>(pelicula);
            dto.Actores = dto.Actores.OrderBy(x => x.Orden).ToList();

            return dto;
        }






        [HttpPost]
        // Aquí se utiliza [FromFrom] para poder recibir el poster (imagen) de la película
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null)
            {
                // GuardarArchivo(..) devuelve un string, que lo asignamos a .Foto (este string es el que se almacenará en la BDD)
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenedor, peliculaCreacionDTO.Poster);
            }

            EscribirOrdenActores(pelicula);

            context.Add(pelicula);
            await context.SaveChangesAsync();
            return NoContent();  // retornamos un 204
        }

        private void EscribirOrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }

        }


    }
}
