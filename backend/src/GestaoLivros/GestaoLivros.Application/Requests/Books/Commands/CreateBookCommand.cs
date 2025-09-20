using GestaoLivros.Application.Requests.Books.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Books.Commands;

public class CreateBookCommand() : CreateBookRequestDto, IRequest<BookResponseDto>;