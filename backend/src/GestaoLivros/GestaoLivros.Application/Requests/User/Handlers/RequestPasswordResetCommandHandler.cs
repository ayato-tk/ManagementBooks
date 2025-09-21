using System.Security.Cryptography;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace GestaoLivros.Application.Requests.User.Handlers;

public class RequestPasswordResetCommandHandler(
    IUserRepository userRepository,
    IPasswordResetRepository passwordResetRepository,
    IEmailService emailService,
    IConfiguration configuration
) : IRequestHandler<RequestPasswordResetCommand>
{
    public async Task Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return;

        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        await passwordResetRepository.AddAsync(new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1)
        });

        await passwordResetRepository.SaveChangesAsync();
        
        //TODO: Implementar um callback no front para chamar no back
        var resetLink =
            $"{configuration["PasswordReset:ResetUrl"] ?? throw new ArgumentNullException("PasswordReset:ResetUrl is not defined")}?token={Uri.EscapeDataString(token)}";
        await emailService.SendEmailAsync(user.Email, "Reset de senha", $"Clique aqui para resetar: {resetLink}");
    }
}