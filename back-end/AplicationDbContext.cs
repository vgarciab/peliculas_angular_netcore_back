using back_end.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end
{
    public class AplicationDbContext : IdentityDbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Con lo siguiente estamos diciendo que, para la Entidad [PeliculasActores], 
            // la PK (compuesta) estará compuesta ActorId + PeliculaId
            modelBuilder.Entity<PeliculasActores>()
                .HasKey(x => new { x.ActorId, x.PeliculaId });

            // Con lo siguiente estamos diciendo que, para la Entidad [PeliculasGeneros], 
            // la PK (compuesta) estará compuesta PeliculaId + GeneroId
            modelBuilder.Entity<PeliculasGeneros>()
                .HasKey(x => new { x.PeliculaId, x.GeneroId });

            // Con lo siguiente estamos diciendo que, para la Entidad [PeliculasCines], 
            // la PK (compuesta) estará compuesta PeliculaId + CineId
            modelBuilder.Entity<PeliculasCines>()
                .HasKey(x => new { x.PeliculaId, x.CineId });


            base.OnModelCreating(modelBuilder);  // No eliminar esta línea. Es importante que se haga la llamada al método de la clase padre.
        }


        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
        public DbSet<PeliculasCines> PeliculasCines { get; set; }
        public DbSet<Rating> Ratings { get; set; }

    }
}
