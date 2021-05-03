using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class PeliculasActores // Entidad de una relación M:N
    {
        public int PeliculaId { get; set; }

        public int ActorId { get; set; }

        // Propiedad de navagación (por su relación M:N)
        public Pelicula Pelicula { get; set; }

        public Actor Actor { get; set; }

        // Queremos guardar también el personaje que interpreta el actor
        [StringLength(maximumLength: 100)]
        public string Personaje  { get; set; }

        // Para mostrar los actores en un orden (descendente) específico (primero el protagonista, etc...según la relevancia del actor en la película)
        public int Orden { get; set; }


    }
}
