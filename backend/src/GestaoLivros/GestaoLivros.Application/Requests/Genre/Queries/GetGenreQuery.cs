using GestaoLivros.Application.Requests.Genre.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Genre.Queries;

public record GetGenreQuery(int Id) : IRequest<GenreResponseDto>;
