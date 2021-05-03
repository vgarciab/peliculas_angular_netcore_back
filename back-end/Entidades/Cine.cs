using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class Cine
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 75)]
        public string Nombre { get; set; }

        // Para querys espaciales (ubicaciones geográficas)
        // Utiliza la librería (paquete NuGet microsoft.entityframeworkcore.sqlserver.nettopologysuite
        public Point Ubicacion { get; set; }

        // Propiedad de navegación para Cines -> Peliculas. Cuando se traiga un Cine (GET), poder las películas que proyecta.
        public List<PeliculasCines> PeliculasCines { get; set; }


    }
}
