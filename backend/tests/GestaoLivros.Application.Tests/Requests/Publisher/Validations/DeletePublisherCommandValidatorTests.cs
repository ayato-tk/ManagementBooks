using FluentValidation.TestHelper;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Requests.Publisher.Validations;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Publisher.Validations
{
    public class DeletePublisherCommandValidatorTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IPublisherRepository> _publisherRepositoryMock;
        private readonly DeletePublisherCommandValidator _validator;

        public DeletePublisherCommandValidatorTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _publisherRepositoryMock = new Mock<IPublisherRepository>();
            var currentUserServiceMock = new Mock<ICurrentUserService>();

            currentUserServiceMock.Setup(x => x.UserId).Returns(1);

            _validator = new DeletePublisherCommandValidator(
                _bookRepositoryMock.Object,
                _publisherRepositoryMock.Object,
                currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_HaveError_WhenPublisherNotFound()
        {
            // Arrange
            var command = new DeletePublisherCommand { Id = 10 };

            _publisherRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id,
                    It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
                .ReturnsAsync((Domain.Entities.Publisher)null!);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Id)
                .WithErrorMessage("A publicadora é inválida");
        }

        [Fact]
        public async Task Handle_HaveError_WhenPublisherHasBooks()
        {
            // Arrange
            var command = new DeletePublisherCommand { Id = 20 };

            _publisherRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id,
                    It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
                .ReturnsAsync(new Domain.Entities.Publisher { Id = command.Id, Name = "Teste" });

            _bookRepositoryMock
                .Setup(r => r.ExistsByPublisherIdAsync(command.Id))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Id)
                .WithErrorMessage("Não é possível excluir esta publicadora pois existem livros associados.");
        }

        [Fact]
        public async Task Handle_NotHaveError_WhenPublisherExistsAndHasNoBooks()
        {
            // Arrange
            var command = new DeletePublisherCommand { Id = 30 };

            _publisherRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id,
                    It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
                .ReturnsAsync(new Domain.Entities.Publisher { Id = command.Id, Name = "Teste" });

            _bookRepositoryMock
                .Setup(r => r.ExistsByPublisherIdAsync(command.Id))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Handle_NotHaveError_WhenPublisherExists()
        {
            // Arrange
            _publisherRepositoryMock.Setup(r => r.GetByIdAsync(1,
                    It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
                .ReturnsAsync(new Domain.Entities.Publisher() { Id = 1, Name = "Teste" });

            var command = new DeletePublisherCommand { Id = 1 };
            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Id);
        }
    }
}