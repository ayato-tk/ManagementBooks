using AutoMapper;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Domain.Entities;

namespace GestaoLivros.Application.Mappers;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookResponseDto>().ReverseMap();
    }
}