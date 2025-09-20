using GestaoLivros.Application.Requests.Publisher.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Publisher.Queries;

public record GetPublisherQuery(int Id) : IRequest<PublisherResponseDto>;