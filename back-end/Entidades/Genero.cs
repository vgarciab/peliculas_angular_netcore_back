using back_end.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        // Propiedad de navegación para Generos -> Peliculas. Cuando se traiga un Genero (GET), poder ver sus películas del Genero.
        public List<PeliculasGeneros> PeliculasGeneros { get; set; }

    }
}
