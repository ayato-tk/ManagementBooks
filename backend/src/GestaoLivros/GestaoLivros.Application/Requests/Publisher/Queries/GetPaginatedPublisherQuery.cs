using GestaoLivros.Application.Requests.Publisher.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Publisher.Queries;

public class GetPaginatedPublishersQuery () : PageFilterDto, IRequest<PaginatedResponse<PublisherResponseDto>>
{
}