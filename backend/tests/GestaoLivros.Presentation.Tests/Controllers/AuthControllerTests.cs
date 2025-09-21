using FluentValidation;
using FluentValidation.Results;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Application.Requests.User.DTOs;
using GestaoLivros.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestaoLivros.Presentation.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IValidator<SignInUserCommand>> _signInValidatorMock = new();
    private readonly Mock<IValidator<CreateUserCommand>> _signUpValidatorMock = new();
    private readonly Mock<IValidator<ResetPasswordCommand>> _resetValidatorMock = new();

    [Fact]
    public async Task SignIn_ShouldReturnOk_WhenValid()
    {
        // Arrange
        var command = new SignInUserCommand("teste@email.com", "senha123");
        _signInValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _mediatorMock.Setup(m => m.Send(command, CancellationToken.None))
            .ReturnsAsync("token123");

        var controller = new AuthController(_mediatorMock.Object);

        // Act
        var result = await controller.SignIn(command, _signInValidatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("token123", okResult.Value);
    }

    [Fact]
    public async Task SignIn_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var command = new SignInUserCommand("", "");
        var failures = new List<ValidationFailure>
        {
            new("Email", "Email inválido")
        };
        _signInValidatorMock.Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult(failures));

        var controller = new AuthController(_mediatorMock.Object);

        // Act
        var result = await controller.SignIn(command, _signInValidatorMock.Object);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<object>>(badRequest.Value);
        Assert.Single(errors);
    }

    [Fact]
    public async Task SignUp_ShouldReturnOk_WhenValid()
    {
        // Arrange
        var command = new CreateUserCommand("Teste", "teste@email.com", "senha123", DateTime.Now);
        _signUpValidatorMock.Setup(v => v.ValidateAsync(command, default))
            .ReturnsAsync(new ValidationResult());

        var userResponse = new UserResponseDto
        {
            Name = "Teste",
            Email = "teste@email.com"
        };

        _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(userResponse);

        var controller = new AuthController(_mediatorMock.Object);

        // Act
        var result = await controller.SignUp(command, _signUpValidatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(userResponse, okResult.Value);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnOk_WhenValid()
    {
        // Arrange
        var command = new ResetPasswordCommand("token123", "novaSenha123");
        _resetValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        var controller = new AuthController(_mediatorMock.Object);

        // Act
        var result = await controller.ResetPassword(command, _resetValidatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Senha alterada com sucesso.", okResult.Value);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var command = new ResetPasswordCommand("token123", "123");
        var failures = new List<ValidationFailure>
        {
            new("NewPassword", "Senha inválida")
        };
        _resetValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
            .ReturnsAsync(new ValidationResult(failures));

        var controller = new AuthController(_mediatorMock.Object);

        // Act
        var result = await controller.ResetPassword(command, _resetValidatorMock.Object);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<object>>(badRequest.Value);
        Assert.Single(errors);
    }
}