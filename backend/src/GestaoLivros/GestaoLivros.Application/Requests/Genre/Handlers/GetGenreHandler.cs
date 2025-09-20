using AutoMapper;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Requests.Genre.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Genre.Handlers;

public class GetGenreHandler(ICurrentUserService currentUserService, IGenreRepository genreRepository, IMapper mapper)
    : IRequestHandler<GetGenreQuery, GenreResponseDto>
{
    public async Task<GenreResponseDto> Handle(GetGenreQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var book = await genreRepository.GetByIdAsync(request.Id, query => query.Where(b => b.UserId == userId));

        return mapper.Map<GenreResponseDto>(book);
    }
}