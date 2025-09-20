using AutoMapper;
using GestaoLivros.Application.Requests.User.DTOs;

namespace GestaoLivros.Application.Mappers;

public class UserProfile  : Profile
{
    public UserProfile()
    {
        CreateMap<Domain.Entities.User, UserResponseDto>().ReverseMap();
    }
}