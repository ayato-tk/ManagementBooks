using FluentValidation;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;

namespace GestaoLivros.Application.Requests.Books.Validations;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateBookCommandValidator(ICurrentUserService currentUserService, IGenreRepository genreRepository,
        IPublisherRepository publisherRepository, IBookRepository bookRepository)
    {
        _genreRepository = genreRepository;
        _publisherRepository = publisherRepository;
        _bookRepository = bookRepository;
        _currentUserService = currentUserService;

        RuleFor(x => x.Id)
            .MustAsync(HasBook)
            .WithMessage("Livro é inválido");

        RuleFor(x => x.Isbn)
            .Length(11, 13)
            .When(x => !string.IsNullOrEmpty(x.Isbn))
            .WithMessage("O Código ISBN deve conter entre 11 e 13 caracteres se informado.");

        RuleFor(x => x.Author)
            .Must(a => string.IsNullOrEmpty(a) || !string.IsNullOrWhiteSpace(a))
            .MinimumLength(3).WithMessage("O Autor deve ter no mínimo 3 caracteres.");

        RuleFor(x => x.Title)
            .Must(t => string.IsNullOrEmpty(t) || !string.IsNullOrWhiteSpace(t))
            .MinimumLength(3).WithMessage("O Título deve ter no mínimo 3 caracteres.");

        RuleFor(x => x.Synopsis)
            .MaximumLength(5000)
            .When(x => !string.IsNullOrEmpty(x.Synopsis))
            .WithMessage("A sinopse deve ter no máximo de 5000 caracteres.");

        RuleFor(x => x.GenreId)
            .MustAsync(HasGenre)
            .When(x => x.GenreId.HasValue)
            .WithMessage("O Gênero é inválido");

        RuleFor(x => x.PublisherId)
            .MustAsync(HasPublisher)
            .When(x => x.PublisherId.HasValue)
            .WithMessage("Publicadora é inválida");
    }

    private async Task<bool> HasGenre(int? genreId, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetByIdAsync(genreId ?? 0,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return genre != null;
    }

    private async Task<bool> HasPublisher(int? publisherId, CancellationToken cancellationToken)
    {
        var publisher = await _publisherRepository.GetByIdAsync(publisherId ?? 0,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return publisher != null;
    }

    private async Task<bool> HasBook(int bookId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(bookId,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return book != null;
    }
}