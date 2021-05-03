using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class PeliculasGeneros // Entidad de una relación M:N
    {
        public int PeliculaId { get; set; }

        public int GeneroId { get; set; }

        // Propiedad de navagación (por su relación M:N)
        public Pelicula Pelicula { get; set; }

        // Propiedad de navagación (por su relación M:N)
        public Genero Genero { get; set; }
    }
}
