using GestaoLivros.Application.Requests.Genre.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Genre.Commands;

public record CreateGenreCommand(string Name) : IRequest<GenreResponseDto>;