using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Requests.Publisher.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Publisher.Handlers;

public class GetPublisherQueryHandler(ICurrentUserService currentUserService, IPublisherRepository publisherRepository, IMapper mapper)
    : IRequestHandler<GetPublisherQuery, PublisherResponseDto>
{
    public async Task<PublisherResponseDto> Handle(GetPublisherQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var book = await publisherRepository.GetByIdAsync(request.Id, query => query.Where(b => b.UserId == userId));

        return mapper.Map<PublisherResponseDto>(book);
    }
}