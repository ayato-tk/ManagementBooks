using FluentValidation;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;

namespace GestaoLivros.Application.Requests.Genre.Validations;

public class DeleteGenreCommandValidator : AbstractValidator<DeleteGenreCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteGenreCommandValidator(IBookRepository bookRepository, IGenreRepository genreRepository, ICurrentUserService currentUserService)
    {
        _bookRepository = bookRepository;
        _genreRepository = genreRepository;
        _currentUserService = currentUserService;
        
        RuleFor(c => c.Id)
            .MustAsync(HasGenre)
            .WithMessage("O Gênero é inválido")
            .MustAsync(HasBook)
            .WithMessage("Não é possível excluir este gênero pois existem livros associados.");
    }

    private async Task<bool> HasBook(int genreId, CancellationToken cancellationToken)
    {
        return !(await _bookRepository.ExistsByGenreIdAsync(genreId));
    }
    
    private async Task<bool> HasGenre(int genreId, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetByIdAsync(genreId,
            query => query.Where(e => e.UserId == _currentUserService.UserId));
        return genre != null;
    }
}