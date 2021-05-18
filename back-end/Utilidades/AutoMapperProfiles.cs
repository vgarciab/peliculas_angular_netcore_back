using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap(); // Con esto ya tenemos el mapeo. (con .ReverseMap() indicamos que sea de doble vía.
            CreateMap<GeneroCreacionDTO, Genero>(); // No es necesario ReverseMap porque nunca va a ser necesario un mapeo en sentido contrario.

            CreateMap<Actor, ActorDTO>().ReverseMap(); // Con esto ya tenemos el mapeo. (con .ReverseMap() indicamos que sea de doble vía.
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());  // Se ignora el campo 'Foto', puesto que se va a tratar de una manera especial


            CreateMap<CineCreacionDTO, Cine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));

            CreateMap<Cine, CineDTO>()
                .ForMember(x => x.Latitud, dto => dto.MapFrom(campo => campo.Ubicacion.Y))
                .ForMember(x => x.Longitud, dto => dto.MapFrom(campo => campo.Ubicacion.X));


            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore())
                .ForMember(x => x.PeliculasGeneros, opciones => opciones.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.PeliculasCines, opciones => opciones.MapFrom(MapearPeliculasCines))
                .ForMember(x => x.PeliculasActores, opciones => opciones.MapFrom(MapearPeliculasActores));


            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember(x => x.Generos, opciones => opciones.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.Actores, opciones => opciones.MapFrom(MapearPeliculasActores))
                .ForMember(x => x.Cines, opciones => opciones.MapFrom(MapearPeliculasCines));

            CreateMap<IdentityUser, UsuarioDTO>();
        }



        private List<CineDTO> MapearPeliculasCines(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<CineDTO>();

            if (pelicula.PeliculasCines != null)
            {
                foreach (var peliculasCines in pelicula.PeliculasCines)
                {
                    resultado.Add(new CineDTO()
                    {
                        Id = peliculasCines.CineId,
                        Nombre = peliculasCines.Cine.Nombre,
                        Latitud = peliculasCines.Cine.Ubicacion.Y,
                        Longitud = peliculasCines.Cine.Ubicacion.X

                    });
                }
            }
            return resultado;
        }



        private List<PeliculaActorDTO> MapearPeliculasActores(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<PeliculaActorDTO>();

            if (pelicula.PeliculasActores != null)
            {
                foreach (var actorPeliculas in pelicula.PeliculasActores)
                {
                    resultado.Add(new PeliculaActorDTO()
                    {
                        Id = actorPeliculas.ActorId,
                        Nombre = actorPeliculas.Actor.Nombre,
                        Foto = actorPeliculas.Actor.Foto,
                        Orden = actorPeliculas.Orden,
                        Personaje = actorPeliculas.Personaje

                    });
                }
            }
            return resultado;
        }


        private List<GeneroDTO> MapearPeliculasGeneros(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<GeneroDTO>();

            if (pelicula.PeliculasGeneros != null)
            {
                foreach (var genero in pelicula.PeliculasGeneros)
                {
                    resultado.Add(new GeneroDTO()
                    {
                        Id = genero.GeneroId,
                        Nombre = genero.Genero.Nombre

                    });
                }
            }
            return resultado;
        }



        private List<PeliculasActores> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();

            if (peliculaCreacionDTO.Actores == null)
            {
                return resultado;
            }


            foreach (var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores()
                {
                    ActorId = actor.Id,
                    Personaje = actor.Personaje

                });
            }

            return resultado;
        }

        private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();

            if (peliculaCreacionDTO.GenerosIds == null)
            {
                return resultado;
            }


            foreach (var id in peliculaCreacionDTO.GenerosIds)
            {
                resultado.Add(new PeliculasGeneros()
                {
                    GeneroId = id
                });
            }

            return resultado;

        }


        private List<PeliculasCines> MapearPeliculasCines(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasCines>();

            if (peliculaCreacionDTO.CinesIds == null)
            {
                return resultado;
            }


            foreach (var id in peliculaCreacionDTO.CinesIds)
            {
                resultado.Add(new PeliculasCines()
                {
                    CineId = id
                });
            }

            return resultado;

        }




    }
}
