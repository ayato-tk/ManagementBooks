using FluentValidation;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;

namespace GestaoLivros.Application.Requests.Publisher.Validations;

public class DeletePublisherCommandValidator : AbstractValidator<DeletePublisherCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeletePublisherCommandValidator(IBookRepository bookRepository, IPublisherRepository publisherRepository, ICurrentUserService currentUserService)
    {
        _bookRepository = bookRepository;
        _publisherRepository = publisherRepository;
        _currentUserService = currentUserService;
        
        RuleFor(c => c.Id)
            .MustAsync(HasPublisher)
            .WithMessage("A publicadora é inválida")
            .MustAsync(HasBook)
            .WithMessage("Não é possível excluir esta publicadora pois existem livros associados.");
    }

    private async Task<bool> HasBook(int publisherId, CancellationToken cancellationToken)
    {
        return !(await _bookRepository.ExistsByPublisherIdAsync(publisherId));
    }
    
    private async Task<bool> HasPublisher(int publisherId, CancellationToken cancellationToken)
    {
        var publisher = await _publisherRepository.GetByIdAsync(publisherId,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return publisher != null;
    }
}