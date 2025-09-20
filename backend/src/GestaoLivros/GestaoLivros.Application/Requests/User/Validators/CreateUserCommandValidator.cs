using FluentValidation;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Domain.Interfaces;

namespace GestaoLivros.Application.User.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    
    
    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("E-mail inválido")
            .MustAsync(UserExists).WithMessage("Usuário já existe.");
            

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
        
    }

    private async Task<bool> UserExists(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null;
    }
    
}