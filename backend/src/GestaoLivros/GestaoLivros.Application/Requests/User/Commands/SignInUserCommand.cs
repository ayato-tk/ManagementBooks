using MediatR;

namespace GestaoLivros.Application.Requests.User.Commands;

public record SignInUserCommand(string Email, string Password) : IRequest<string>;