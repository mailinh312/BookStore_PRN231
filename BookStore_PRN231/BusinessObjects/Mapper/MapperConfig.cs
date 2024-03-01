using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Mapper
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<BookDTO, Book>().ReverseMap();
            CreateMap<RegisterDTO, AppUser>().ReverseMap();
        }
    }
}
