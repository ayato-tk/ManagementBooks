using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Publisher.Handlers;

public class CreatePublisherCommandHandler(
    IPublisherRepository publisherRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<CreatePublisherCommand, PublisherResponseDto>
{
    public async Task<PublisherResponseDto> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = new Domain.Entities.Publisher { Name = request.name, UserId = currentUserService.UserId };

        await publisherRepository.AddAsync(publisher);
        await publisherRepository.SaveChangesAsync();

        return mapper.Map<PublisherResponseDto>(publisher);
    }
}