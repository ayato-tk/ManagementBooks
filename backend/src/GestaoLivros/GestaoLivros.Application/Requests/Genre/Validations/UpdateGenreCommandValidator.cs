using FluentValidation;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;

namespace GestaoLivros.Application.Requests.Genre.Validations;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    private readonly IGenreRepository _genreRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateGenreCommandValidator(ICurrentUserService currentUserService, IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
        _currentUserService = currentUserService;

        RuleFor(x => x.Name)
            .Must(t => string.IsNullOrEmpty(t) || !string.IsNullOrWhiteSpace(t))
            .MinimumLength(3).WithMessage("O Nome deve ter no mínimo 3 caracteres.");

        RuleFor(x => x.Id)
            .MustAsync(HasGenre).WithMessage("O Gênero é inválido");
    }

    private async Task<bool> HasGenre(int genreId, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetByIdAsync(genreId,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return genre != null;
    }
}