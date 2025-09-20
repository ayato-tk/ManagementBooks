using FluentValidation;
using GestaoLivros.Application.Requests.User.Commands;

namespace GestaoLivros.Application.Requests.User.Validators;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
    }
}