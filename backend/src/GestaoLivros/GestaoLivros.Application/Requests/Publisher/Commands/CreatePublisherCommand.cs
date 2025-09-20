using GestaoLivros.Application.Requests.Publisher.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Publisher.Commands;

public record CreatePublisherCommand(string name) : IRequest<PublisherResponseDto>;