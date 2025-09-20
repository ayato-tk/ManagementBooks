using AutoMapper;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Requests.Genre.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Genre.Handlers;

public class GetPaginatedGenresHandler(
    ICurrentUserService currentUserService,
    IGenreRepository genreRepository,
    IMapper mapper)
    : IRequestHandler<GetPaginatedGenresQuery, PaginatedResponse<GenreResponseDto>>
{
    public async Task<PaginatedResponse<GenreResponseDto>> Handle(GetPaginatedGenresQuery request,
        CancellationToken cancellationToken)
    {
        var page = request.Page;
        var pageSize = request.PageSize;

        var userId = currentUserService.UserId;

        var (items, totalCount) = await genreRepository.GetPaginatedAsync(page, pageSize, request.Search,
            query => query.Where(u => u.UserId == userId), b => b.Id.ToString(), b => b.Name);

        var data = mapper.Map<List<GenreResponseDto>>(items);

        return new PaginatedResponse<GenreResponseDto>
        {
            Data = data,
            Page = request.Page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}