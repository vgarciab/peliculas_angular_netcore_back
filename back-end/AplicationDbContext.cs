using back_end.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }

    }
}
