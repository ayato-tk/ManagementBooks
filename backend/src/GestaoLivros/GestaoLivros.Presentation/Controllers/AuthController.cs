using FluentValidation;
using GestaoLivros.Application.Requests.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestaoLivros.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInUserCommand request,
        [FromServices] IValidator<SignInUserCommand> validator)
    {
        var validationRes = await validator.ValidateAsync(request);

        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors.Select(x => new
            {
                Field = x.PropertyName,
                Message = x.ErrorMessage
            }));
        }

        var token = await mediator.Send(request);
        return Ok(token);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserCommand request,
        [FromServices] IValidator<CreateUserCommand> validator)
    {
        var validationRes = await validator.ValidateAsync(request);

        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors.Select(x => new
            {
                Field = x.PropertyName,
                Message = x.ErrorMessage
            }));
        }

        var user = await mediator.Send(request);
        return Ok(user);
    }

    [HttpPost("request-reset")]
    public async Task<IActionResult> RequestReset([FromBody] RequestPasswordResetCommand command)
    {
        await mediator.Send(command);
        return Ok("E-mail enviado, você receberá instruções.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command,
        [FromServices] IValidator<ResetPasswordCommand> validator)
    {
        var validationRes = await validator.ValidateAsync(command);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors.Select(x => new
            {
                Field = x.PropertyName,
                Message = x.ErrorMessage
            }));
        }

        await mediator.Send(command);
        return Ok("Senha alterada com sucesso.");
    }
}