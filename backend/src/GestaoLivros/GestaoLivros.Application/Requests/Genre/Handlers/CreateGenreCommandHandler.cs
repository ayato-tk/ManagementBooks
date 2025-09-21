using AutoMapper;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Genre.Handlers;

public class CreateGenreCommandHandler(
    IGenreRepository genreRepository,
    ICurrentUserService currentUserService,
    IMapper mapper
) : IRequestHandler<CreateGenreCommand, GenreResponseDto>
{
    public async Task<GenreResponseDto> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = new Domain.Entities.Genre { Name = request.Name, UserId = currentUserService.UserId };

        await genreRepository.AddAsync(genre);
        await genreRepository.SaveChangesAsync();

        return mapper.Map<GenreResponseDto>(genre);
    }
}