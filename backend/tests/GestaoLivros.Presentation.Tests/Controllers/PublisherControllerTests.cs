using FluentValidation;
using FluentValidation.Results;
using GestaoLivros.Application;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Requests.Publisher.Queries;
using GestaoLivros.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestaoLivros.Presentation.Tests.Controllers
{
    public class PublisherControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<IValidator<UpdatePublisherCommand>> _updateValidatorMock = new();
        private readonly Mock<IValidator<DeletePublisherCommand>> _deleteValidatorMock = new();

        private readonly PublisherResponseDto _publisher = new()
        {
            Id = 1,
            Name = "Teste"
        };

        [Fact]
        public async Task CreatePublisher_ShouldReturnOk()
        {
            // Arrange
            var command = new CreatePublisherCommand(_publisher.Name);
            _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(_publisher);

            var controller = new PublisherController(_mediatorMock.Object);

            // Act
            var result = await controller.CreatePublisher(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_publisher, okResult.Value);
        }

        [Fact]
        public async Task GetPublishers_ShouldReturnOk()
        {
            // Arrange
            var query = new GetPaginatedPublishersQuery { Page = 1, PageSize = 10 };
            var paginatedPublishers = new PaginatedResponse<PublisherResponseDto>();
            _mediatorMock.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(paginatedPublishers);

            var controller = new PublisherController(_mediatorMock.Object);

            // Act
            var result = await controller.GetPublishers(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(paginatedPublishers, okResult.Value);
        }

        [Fact]
        public async Task GetPublisher_ShouldReturnOk()
        {
            // Arrange
            var query = new GetPublisherQuery(1);
            _mediatorMock.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(_publisher);

            var controller = new PublisherController(_mediatorMock.Object);

            // Act
            var result = await controller.GetPublishers(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_publisher, okResult.Value);
        }

        [Fact]
        public async Task UpdatePublisher_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var command = new UpdatePublisherCommand { Id = 1, Name = "Teste2" };
            _updateValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(_publisher);

            var controller = new PublisherController(_mediatorMock.Object);

            // Act
            var result = await controller.UpdatePublisher(command, _updateValidatorMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_publisher, okResult.Value);
        }

        [Fact]
        public async Task DeletePublisher_ShouldReturnOk_WhenValid()
        {
            // Arrange
            var command = new DeletePublisherCommand { Id = 1 };
            _deleteValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(_publisher);

            var controller = new PublisherController(_mediatorMock.Object);

            // Act
            var result = await controller.DeletePublisher(command, _deleteValidatorMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_publisher, okResult.Value);
        }
    }
}