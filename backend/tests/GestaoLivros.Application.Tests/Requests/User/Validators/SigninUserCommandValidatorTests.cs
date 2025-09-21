using FluentValidation.TestHelper;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Application.Requests.User.Validators;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.User.Validators;

public class SigninUserCommandValidatorTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly SignInUserCommandValidator _validator;

    private readonly Domain.Entities.User _user = new()
        { Name = "", Email = "teste@email.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };

    public SigninUserCommandValidatorTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _validator = new SignInUserCommandValidator(_userRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldHaveError_WhenEmailIsEmpty()
    {
        var command = new SignInUserCommand("", "123456");
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Email é obrigatório");
    }

    [Fact]
    public async Task Handle_ShouldHaveError_WhenEmailIsInvalid()
    {
        var command = new SignInUserCommand("invalid-email", "123456");
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("E-mail inválido");
    }

    [Fact]
    public async Task Handle_ShouldHaveError_WhenUserDoesNotExist()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync("teste@email.com"))
            .ReturnsAsync((Domain.Entities.User)null!);

        var command = new SignInUserCommand("teste@email.com", "123456");
        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Usuário não encontrado.");
    }

    [Fact]
    public async Task Handle_ShouldHaveError_WhenPasswordIsIncorrect()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync(_user.Email)).ReturnsAsync(_user);

        var command = new SignInUserCommand(_user.Email, "senhaErrada");
        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c)
            .WithErrorMessage("Email ou senha inválidos.");
    }

    [Fact]
    public async Task Handle_ShouldNotHaveError_WhenEmailAndPasswordAreCorrect()
    {
        _userRepoMock.Setup(r => r.GetByEmailAsync(_user.Email)).ReturnsAsync(_user);

        var command = new SignInUserCommand(_user.Email, "password");
        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}