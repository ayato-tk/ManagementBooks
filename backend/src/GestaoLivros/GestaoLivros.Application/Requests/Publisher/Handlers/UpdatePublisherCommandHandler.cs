using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Publisher.Handlers;

public class UpdatePublisherCommandHandler(
    IPublisherRepository publisherRepository,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IRequestHandler<UpdatePublisherCommand, PublisherResponseDto>
{
    public async Task<PublisherResponseDto> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var publisher =
            await publisherRepository.GetByIdAsync(request.Id, query => query.Where(b => b.UserId == userId));
        if (publisher == null)
            throw new KeyNotFoundException("Publicadora não encontrada");

        publisher.Name = request.Name ?? publisher.Name;

        await publisherRepository.UpdateAsync(publisher);
        await publisherRepository.SaveChangesAsync();
        return mapper.Map<PublisherResponseDto>(publisher);
    }
}