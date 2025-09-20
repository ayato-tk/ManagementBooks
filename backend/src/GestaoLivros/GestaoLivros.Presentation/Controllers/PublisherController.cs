using FluentValidation;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Requests.Publisher.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoLivros.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PublisherController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePublisher([FromBody] CreatePublisherCommand command)
    {
        var publisher = await mediator.Send(command);
        return Ok(publisher);
    }

    [HttpGet]
    public async Task<IActionResult> GetPublishers([FromQuery] GetPaginatedPublishersQuery query)
    {
        var publisher = await mediator.Send(query);
        return Ok(publisher);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPublishers([FromRoute] GetPublisherQuery query)
    {
        var publisher = await mediator.Send(query);
        return Ok(publisher);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdatePublisher([FromBody] UpdatePublisherCommand query,
        [FromServices] IValidator<UpdatePublisherCommand> validator)
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

        var publisher = await mediator.Send(query);
        return Ok(publisher);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePublisher([FromRoute] DeletePublisherCommand query,
        [FromServices] IValidator<DeletePublisherCommand> validator)
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

        var publisher = await mediator.Send(query);
        return Ok(publisher);
    }
}