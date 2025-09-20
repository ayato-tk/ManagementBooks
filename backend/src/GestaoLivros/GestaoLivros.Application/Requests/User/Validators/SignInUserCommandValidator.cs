using FluentValidation;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Domain.Interfaces;

namespace GestaoLivros.Application.Requests.User.Validators;

public class SignInUserCommandValidator : AbstractValidator<SignInUserCommand>
{
    private readonly IUserRepository _userRepository;
    
    
    public SignInUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("E-mail inválido")
            .MustAsync(UserExists).WithMessage("Usuário não encontrado.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
        
        RuleFor(x => x)
            .MustAsync(ValidatePassword)
            .WithMessage("Email ou senha inválidos.");
    }

    private async Task<bool> UserExists(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user != null;
    }
    
    private async Task<bool> ValidatePassword(SignInUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        return user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
    }
    
}