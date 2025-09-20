using GestaoLivros.Application.Requests.Books.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Books.Queries;

public class GetPaginatedBooksQuery() : PageFilterDto, IRequest<PaginatedResponse<BookResponseDto>>;