using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Requests.Publisher.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Publisher.Handlers;

public class GetPaginatedPublishersQueryHandler(
    ICurrentUserService currentUserService,
    IPublisherRepository publisherRepository,
    IMapper mapper)
    : IRequestHandler<GetPaginatedPublishersQuery, PaginatedResponse<PublisherResponseDto>>
{
    public async Task<PaginatedResponse<PublisherResponseDto>> Handle(GetPaginatedPublishersQuery request,
        CancellationToken cancellationToken)
    {
        var page = request.Page;
        var pageSize = request.PageSize;

        var userId = currentUserService.UserId;

        var (items, totalCount) = await publisherRepository.GetPaginatedAsync(page, pageSize, request.Search,
            query => query.Where(u => u.UserId == userId), b => b.Id.ToString(), b => b.Name);

        var data = mapper.Map<List<PublisherResponseDto>>(items);

        return new PaginatedResponse<PublisherResponseDto>
        {
            Data = data,
            Page = request.Page,
            PageSize = pageSize,
            TotalItems = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}