using AutoMapper;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Application.Requests.User.DTOs;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.User.Handlers;

public class CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<CreateUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash =  BCrypt.Net.BCrypt.HashPassword(request.Password),
            Birthdate = request.birthDate
        };
        
        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return mapper.Map<UserResponseDto>(user);
    }
}