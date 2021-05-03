using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 300)]
        public string Nombre { get; set; }

        public string Resumen { get; set; }

        public string Traile { get; set; }

        public bool EnCines { get; set; }

        public DateTime FechaLanzamiento { get; set; }

        public string Poster { get; set; }

        // Propiedad de navegación para Peliculas -> Actores. Cuando se traiga una película (GET), también se puedan traer a sus actores
        public List<PeliculasActores> PeliculasActores { get; set; }

        // Propiedad de navegación para Peliculas -> Generos.
        public List<PeliculasGeneros> PeliculasGeneros { get; set; }

        // Propiedad de navegación para Peliculas -> Cines.
        public List<PeliculasCines> PeliculasCines { get; set; }

    }
}
