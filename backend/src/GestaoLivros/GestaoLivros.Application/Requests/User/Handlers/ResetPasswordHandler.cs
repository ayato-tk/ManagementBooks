using System.ComponentModel.DataAnnotations;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.User.Handlers;

public class ResetPasswordHandler(IUserRepository userRepository, IPasswordResetRepository resetRepository)
    : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var tokenEntry = await resetRepository.GetByTokenAsync(request.Token);
        if (tokenEntry == null || tokenEntry.Used || tokenEntry.Expiration < DateTime.UtcNow)
            throw new ValidationException("Token inválido ou expirado.");

        var user = await userRepository.GetByIdAsync(tokenEntry.UserId);
        if (user == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

        tokenEntry.Used = true;

        await userRepository.SaveChangesAsync();
        await resetRepository.SaveChangesAsync();
    }
}