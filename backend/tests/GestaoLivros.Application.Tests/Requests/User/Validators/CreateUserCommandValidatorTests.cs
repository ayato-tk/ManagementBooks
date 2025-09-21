using FluentValidation.TestHelper;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Application.User.Validators;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.User.Validators;

public class CreateUserCommandValidatorTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _validator = new CreateUserCommandValidator(_userRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldHaveError_WhenEmailIsEmpty()
    {
        var command = new CreateUserCommand("", "password123", "Teste", DateTime.UtcNow);
        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public async Task Handle_ShouldHaveError_WhenEmailIsInvalid()
    {
        var command = new CreateUserCommand("invalid-email", "password123", "Teste", DateTime.UtcNow);
        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email);
    }
    
     [Fact]
    public async Task Handle_ShouldHaveError_WhenEmailAlreadyExists()
    {
        const string email = "teste@email.com";
        _userRepoMock.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(new Domain.Entities.User { Name = "Teste", Email = email });

        var command = new CreateUserCommand("Teste", email, "password123", DateTime.UtcNow);
        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Usuário já existe.");
    }

    [Fact]
    public async Task Handle_ShouldHaveError_WhenPasswordIsTooShort()
    {
        var command = new CreateUserCommand("teste@email.com", "123", "Teste", DateTime.UtcNow);
        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public async Task Handle_ShouldPass_WhenDataIsValid()
    {
        const string email = "valid@email.com";
        _userRepoMock.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync((Domain.Entities.User)null!);

        var command = new CreateUserCommand("Teste", email, "password123", DateTime.UtcNow);
        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}