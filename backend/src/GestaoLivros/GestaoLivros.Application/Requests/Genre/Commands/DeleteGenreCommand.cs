using GestaoLivros.Application.Requests.Genre.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Genre.Commands;

public class DeleteGenreCommand : GenreRequestDto, IRequest<GenreResponseDto>;