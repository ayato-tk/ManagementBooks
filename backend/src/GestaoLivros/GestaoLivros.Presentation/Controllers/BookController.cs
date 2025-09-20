using FluentValidation;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoLivros.Presentation.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class BookController(IMediator mediator) : ControllerBase
{
    //TODO: Criar o find pelo search
    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] GetPaginatedBooksQuery query)
    {
        var book = await mediator.Send(query);
        return Ok(book);
    }
    
    [HttpGet("report")]
    public async Task<IActionResult> GetBooksReport()
    {
        var pdfBytes = await mediator.Send(new GetBooksReportQuery());
        return File(pdfBytes, "application/pdf", "relatorio_livros.pdf");
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook([FromRoute] GetBookQuery query)
    {
        var book = await mediator.Send(query);
        return Ok(book);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateBook([FromBody] UpdateBookCommand query,
        [FromServices] IValidator<UpdateBookCommand> validator)
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

        var book = await mediator.Send(query);
        return Ok(book);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook([FromRoute] DeleteBookCommand query,
        [FromServices] IValidator<DeleteBookCommand> validator)
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

        var book = await mediator.Send(query);
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command,
        [FromServices] IValidator<CreateBookCommand> validator)
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

        var book = await mediator.Send(command);
        return Ok(book);
    }
}