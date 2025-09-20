using FluentValidation;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Requests.Genre.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoLivros.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class GenreController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateGenre([FromBody] CreateGenreCommand command)
    {
        var genre = await mediator.Send(command);
        return Ok(genre);
    }

    [HttpGet]
    public async Task<IActionResult> GetGenres([FromQuery] GetPaginatedGenresQuery query)
    {
        var genre = await mediator.Send(query);
        return Ok(genre);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenres([FromRoute] GetGenreQuery query)
    {
        var genre = await mediator.Send(query);
        return Ok(genre);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateGenre([FromBody] UpdateGenreCommand query,
        [FromServices] IValidator<UpdateGenreCommand> validator)
    {
        var validationRes = await validator.ValidateAsync(query);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors.Select(x => new
            {
                Field = x.PropertyName,
                Message = x.ErrorMessage
            }));
        }

        var genre = await mediator.Send(query);
        return Ok(genre);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre([FromRoute] DeleteGenreCommand query,
        [FromServices] IValidator<DeleteGenreCommand> validator)
    {
        var validationRes = await validator.ValidateAsync(query);
        if (!validationRes.IsValid)
        {
            return BadRequest(validationRes.Errors.Select(x => new
            {
                Field = x.PropertyName,
                Message = x.ErrorMessage
            }));
        }

        var genre = await mediator.Send(query);
        return Ok(genre);
    }
}