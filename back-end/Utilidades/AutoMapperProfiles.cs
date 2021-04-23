using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap(); // Con esto ya tenemos el mapeo. (con .ReverseMap() indicamos que sea de doble vía.
            CreateMap<GeneroCreacionDTO, Genero>(); // No es necesario ReverseMap porque nunca va a ser necesario un mapeo en sentido contrario.

            CreateMap<Actor, ActorDTO>().ReverseMap(); // Con esto ya tenemos el mapeo. (con .ReverseMap() indicamos que sea de doble vía.
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());  // Se ignora el campo 'Foto', puesto que se va a tratar de una manera especial


        }
    }
}
