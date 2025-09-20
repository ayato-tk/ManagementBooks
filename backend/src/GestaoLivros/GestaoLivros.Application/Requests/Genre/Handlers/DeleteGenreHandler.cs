using AutoMapper;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Genre.Handlers;

public class DeleteGenreHandler(IGenreRepository genreRepository, ICurrentUserService currentUserService, IMapper mapper)
    : IRequestHandler<DeleteGenreCommand, GenreResponseDto>
{
    public async Task<GenreResponseDto> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var genre = await genreRepository.GetByIdAsync(request.Id, query => query.Where(b => b.UserId == userId));
        if (genre == null)
            throw new KeyNotFoundException("Gênero não encontrado");

        await genreRepository.DeleteAsync(genre);
        await genreRepository.SaveChangesAsync();
        return mapper.Map<GenreResponseDto>(genre);
    }
}