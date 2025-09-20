using GestaoLivros.Application.Requests.Publisher.DTOs;
using MediatR;

namespace GestaoLivros.Application.Requests.Publisher.Commands;

public class UpdatePublisherCommand() : PublisherRequestDto, IRequest<PublisherResponseDto>;