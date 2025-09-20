using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.DTOs;

namespace GestaoLivros.Application.Mappers;

public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<Domain.Entities.Publisher, PublisherResponseDto>().ReverseMap();
    }
}