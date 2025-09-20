using AutoMapper;
using GestaoLivros.Application.Requests.Genre.DTOs;

namespace GestaoLivros.Application.Mappers;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Domain.Entities.Genre, GenreResponseDto>().ReverseMap();
    }
}