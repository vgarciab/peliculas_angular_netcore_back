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
            CreateMap<GeneroCreacionDTO, Genero>(); // No es necesario ReverseMap porque nunca va a ser necesario un mapeo en sentido contrario.

        }
    }
}
