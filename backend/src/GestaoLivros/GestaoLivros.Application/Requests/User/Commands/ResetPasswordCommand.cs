using MediatR;

namespace GestaoLivros.Application.Requests.User.Commands;

public record ResetPasswordCommand(string Token, string NewPassword) : IRequest;