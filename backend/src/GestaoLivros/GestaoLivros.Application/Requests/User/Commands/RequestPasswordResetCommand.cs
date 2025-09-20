using MediatR;

namespace GestaoLivros.Application.Requests.User.Commands;

public record RequestPasswordResetCommand(string Email) : IRequest;
