using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 200)]
        public string Nombre { get; set; }

        public string Biografia { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string  Foto { get; set; }

        // Propiedad de navegación para Actores -> Peliculas. Cuando se traiga una actor (GET), poder ver sus películas en las que ha actuado.
        public List<PeliculasActores> PeliculasActores { get; set; }

    }
}
