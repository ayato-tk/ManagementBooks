using GestaoLivros.Application.Requests.Books.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Books.Queries;

public record GetBookQuery(int Id) : IRequest<BookResponseDto>;