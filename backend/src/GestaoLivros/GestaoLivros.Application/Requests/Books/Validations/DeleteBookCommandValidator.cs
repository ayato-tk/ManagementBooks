using FluentValidation;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;

namespace GestaoLivros.Application.Requests.Books.Validations;

public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteBookCommandValidator(IBookRepository bookRepository, ICurrentUserService currentUserService)
    {
        _bookRepository = bookRepository;
        _currentUserService = currentUserService;

        RuleFor(c => c.Id)
            .MustAsync(HasBook)
            .WithMessage("O Livro é inválido");
    }
    
    private async Task<bool> HasBook(int bookId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(bookId,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return book != null;
    }
}