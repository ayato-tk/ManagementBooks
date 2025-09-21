using FluentValidation.TestHelper;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Requests.Genre.Validations;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Genre.Validations
{
    public class DeleteGenreCommandValidatorTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly DeleteGenreCommandValidator _validator;

        public DeleteGenreCommandValidatorTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _genreRepositoryMock = new Mock<IGenreRepository>();
            var currentUserServiceMock = new Mock<ICurrentUserService>();

            currentUserServiceMock.Setup(x => x.UserId).Returns(1);

            _validator = new DeleteGenreCommandValidator(
                _bookRepositoryMock.Object,
                _genreRepositoryMock.Object,
                currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_HaveError_WhenGenreNotFound()
        {
            // Arrange
            var command = new DeleteGenreCommand { Id = 10 };

            _genreRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id,
                    It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
                .ReturnsAsync((Domain.Entities.Genre)null!);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Id)
                .WithErrorMessage("O Gênero é inválido");
        }

        [Fact]
        public async Task Handle_HaveError_WhenGenreHasBooks()
        {
            // Arrange
            var command = new DeleteGenreCommand { Id = 20 };

            _genreRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id,
                    It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
                .ReturnsAsync(new Domain.Entities.Genre { Id = command.Id, Name = "Teste" });

            _bookRepositoryMock
                .Setup(r => r.ExistsByGenreIdAsync(command.Id))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Id)
                .WithErrorMessage("Não é possível excluir este gênero pois existem livros associados.");
        }

        [Fact]
        public async Task Handle_NotHaveError_WhenGenreExistsAndHasNoBooks()
        {
            // Arrange
            var command = new DeleteGenreCommand { Id = 30 };

            _genreRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id,
                    It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
                .ReturnsAsync(new Domain.Entities.Genre { Id = command.Id, Name = "Teste" });

            _bookRepositoryMock
                .Setup(r => r.ExistsByGenreIdAsync(command.Id))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Handle_NotHaveError_WhenGenreExists()
        {
            // Arrange
            _genreRepositoryMock.Setup(r => r.GetByIdAsync(1,
                    It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
                .ReturnsAsync(new Domain.Entities.Genre() { Id = 1, Name = "Teste" });

            var command = new DeleteGenreCommand { Id = 1 };
            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Id);
        }
    }
}