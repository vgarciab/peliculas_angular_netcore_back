using back_end.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.DTOs
{
    public class PeliculaCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 300)]
        public string Titulo { get; set; }

        public string Resumen { get; set; }

        public string Trailer { get; set; }

        public bool EnCines { get; set; }

        public DateTime FechaLanzamiento { get; set; }

        public IFormFile Poster { get; set; } // Del front-end vamos a querer recibir un archivo, como tal

        // Otra cosa que también vamos a recibir va a ser el listado de Géneros, el listado de Actores y el listado de salas de Cine,
        // que se corresponderán con la película que se esté tratando. Para ello se utiliza un Model Binder personalizado.
        // Para ello se crea la Class 'TypeBinder (que implementa IModelBinder)', que será nuestro ModelBinder personalizado.
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CinesIds { get; set; }

        // En el caso de los Actores, va a ser un listado de un tipo complejo,
        // ya que un Actor es más que un ID; va a ser el personaje, el orden de
        // relevancia en la película...y hay que capturar esa información (por lo menos la del personaje)
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculaCreacionDTO>>))]
        public List<ActorPeliculaCreacionDTO> Actores { get; set; }



    }

}
