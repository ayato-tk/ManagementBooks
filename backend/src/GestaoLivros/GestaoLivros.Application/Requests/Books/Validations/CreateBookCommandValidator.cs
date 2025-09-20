using FluentValidation;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;

namespace GestaoLivros.Application.Requests.Books.Validations;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    private readonly IGenreRepository _genreRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateBookCommandValidator(IGenreRepository genreRepository,
        IPublisherRepository publisherRepository, ICurrentUserService currentUserService)
    {
        _genreRepository = genreRepository;
        _publisherRepository = publisherRepository;
        _currentUserService = currentUserService;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("O Título é obrigatório");

        RuleFor(x => x.Isbn)
            .NotEmpty().WithMessage("ISBN é obrigatório")
            .Must(x => x.Length is 10 or 13).WithMessage("O Código ISBN deve conter 10 a 13 caracteres");

        RuleFor(x => x.Synopsis)
            .MaximumLength(5000).WithMessage("A sinopse deve ter no máximo de 5000 caracteres.");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("O Autor é obrigatório");

        RuleFor(x => x.GenreId)
            .NotEmpty().WithMessage("O Gênero é obrigatório")
            .MustAsync(HasGenre).WithMessage("Gênero é inválido");

        RuleFor(x => x.PublisherId)
            .NotEmpty().WithMessage("A Publicadora é obrigatória")
            .MustAsync(HasPublisher).WithMessage("Publicadora é inválida");
    }

    private async Task<bool> HasGenre(int genreId, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetByIdAsync(genreId,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return genre != null;
    }

    private async Task<bool> HasPublisher(int publisherId, CancellationToken cancellationToken)
    {
        var publisher = await _publisherRepository.GetByIdAsync(publisherId,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return publisher != null;
    }
}