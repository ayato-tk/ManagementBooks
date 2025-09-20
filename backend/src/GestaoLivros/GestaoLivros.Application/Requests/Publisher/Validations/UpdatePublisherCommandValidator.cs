using FluentValidation;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;

namespace GestaoLivros.Application.Requests.Publisher.Validations;

public class UpdatePublisherCommandValidator : AbstractValidator<UpdatePublisherCommand>
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdatePublisherCommandValidator(ICurrentUserService currentUserService, IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
        _currentUserService = currentUserService;

        RuleFor(x => x.Name)
            .Must(t => string.IsNullOrEmpty(t) || !string.IsNullOrWhiteSpace(t))
            .MinimumLength(3).WithMessage("O Nome deve ter no mínimo 3 caracteres.");

        RuleFor(x => x.Id)
            .MustAsync(HasPublisher).WithMessage("A publicadora é inválida");
    }

    private async Task<bool> HasPublisher(int publisherId, CancellationToken cancellationToken)
    {
        var publisher = await _publisherRepository.GetByIdAsync(publisherId,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return publisher != null;
    }
}