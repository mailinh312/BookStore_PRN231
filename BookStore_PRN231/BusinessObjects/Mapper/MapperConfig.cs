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
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.AuthorName : null))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ReverseMap();

            CreateMap<Book, BookCreateDto>().ReverseMap();

            CreateMap<AppUser, UserDto>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Category, CategoryCreateDto>().ReverseMap();

            CreateMap<Author, AuthorDto>().ReverseMap();

            CreateMap<Author, AuthorCreateDto>().ReverseMap();

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.StatusName : null))
                .ReverseMap();

            CreateMap<Order, OrderCreateDto>().ReverseMap();

            CreateMap<OrderDetail, OrderDetailCreateDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : null))
                .ForMember(dest => dest.BookPrice, opt => opt.MapFrom(src => src.Book != null ? src.Book.Price : null))
                .ReverseMap();

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : null))
                .ReverseMap();

            CreateMap<Import, ImportDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ReverseMap();

            CreateMap<Import, ImportCreateDto>().ReverseMap();
            CreateMap<ImportDetail, ImportDetailCreateDto>().ReverseMap();
            CreateMap<ImportDetail, ImportDetailDto>()
               .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : null))
               .ReverseMap();
        }
    }
}
