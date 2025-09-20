using AutoMapper;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Requests.Books.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Books.Handlers;

public class GetPaginatedBookHandler(
    IBookRepository bookRepository,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetBookQuery, BookResponseDto>
{
    public async Task<BookResponseDto> Handle(GetBookQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var book = await bookRepository.GetByIdAsync(request.Id, query => query.Where(b => b.UserId == userId));

        return mapper.Map<BookResponseDto>(book);
    }
}