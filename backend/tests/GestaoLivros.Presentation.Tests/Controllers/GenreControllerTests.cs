using FluentValidation;
using FluentValidation.Results;
using GestaoLivros.Application;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Requests.Genre.Queries;
using GestaoLivros.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestaoLivros.Presentation.Tests.Controllers
{
    public class GenreControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<IValidator<UpdateGenreCommand>> _updateValidatorMock = new();
        private readonly Mock<IValidator<DeleteGenreCommand>> _deleteValidatorMock = new();

        private readonly GenreResponseDto _genre = new()
        {
            Id = 1,
            Name = "Aventura"
        };

        [Fact]
        public async Task CreateGenre_ShouldReturnOk()
        {
            // Arrange
            var command = new CreateGenreCommand(_genre.Name);
            _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(_genre);

            var controller = new GenreController(_mediatorMock.Object);

            // Act
            var result = await controller.CreateGenre(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_genre, okResult.Value);
        }

        [Fact]
        public async Task GetGenres_ShouldReturnOk()
        {
            // Arrange
            var query = new GetPaginatedGenresQuery { Page = 1, PageSize = 10 };
            var paginatedGenres = new PaginatedResponse<GenreResponseDto>();
            _mediatorMock.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(paginatedGenres);

            var controller = new GenreController(_mediatorMock.Object);

            // Act
            var result = await controller.GetGenres(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(paginatedGenres, okResult.Value);
        }

        [Fact]
        public async Task GetGenre_ShouldReturnOk()
        {
            // Arrange
            var query = new GetGenreQuery(1);
            _mediatorMock.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(_genre);

            var controller = new GenreController(_mediatorMock.Object);

            // Act
            var result = await controller.GetGenres(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_genre, okResult.Value);
        }

        [Fact]
        public async Task UpdateGenre_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var command = new UpdateGenreCommand { Id = 1, Name = "Ficção" };
            _updateValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(_genre);

            var controller = new GenreController(_mediatorMock.Object);

            // Act
            var result = await controller.UpdateGenre(command, _updateValidatorMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_genre, okResult.Value);
        }

        [Fact]
        public async Task DeleteGenre_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var command = new DeleteGenreCommand { Id = 1 };
            _deleteValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(_genre);

            var controller = new GenreController(_mediatorMock.Object);

            // Act
            var result = await controller.DeleteGenre(command, _deleteValidatorMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_genre, okResult.Value);
        }
    }
}