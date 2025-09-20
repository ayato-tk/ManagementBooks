using GestaoLivros.Application.Requests.Genre.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Genre.Queries;

public class GetPaginatedGenresQuery () : PageFilterDto, IRequest<PaginatedResponse<GenreResponseDto>>
{
    
}