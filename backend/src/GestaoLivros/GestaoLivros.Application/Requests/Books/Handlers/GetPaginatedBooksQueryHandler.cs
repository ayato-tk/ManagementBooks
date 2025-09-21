using AutoMapper;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Requests.Books.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Books.Handlers;

public class GetPaginatedBooksQueryHandler(
    IBookRepository bookRepository,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetPaginatedBooksQuery, PaginatedResponse<BookResponseDto>>
{
    public async Task<PaginatedResponse<BookResponseDto>> Handle(GetPaginatedBooksQuery request,
        CancellationToken cancellationToken)
    {
        var page = request.Page;
        var pageSize = request.PageSize;

        var userId = currentUserService.UserId;

        var (items, totalCount) = await bookRepository.GetPaginatedAsync(page, pageSize, request.Search,
            query => query.Where(b => b.UserId == userId), b => b.Title!, b => b.Author, b => b.ISBN!,
            b => b.Publisher!.Name!, b => b.Genre!.Name!);

        var data = mapper.Map<List<BookResponseDto>>(items);

        return new PaginatedResponse<BookResponseDto>
        {
            Data = data,
            Page = request.Page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}