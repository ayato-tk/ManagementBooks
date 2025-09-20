using GestaoLivros.Application.Requests.User.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.User.Commands;

public record CreateUserCommand(string Name, string Email, string Password, DateTime birthDate) : IRequest<UserResponseDto>;