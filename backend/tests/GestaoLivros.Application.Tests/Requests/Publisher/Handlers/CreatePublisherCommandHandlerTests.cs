using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Requests.Publisher.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Publisher.Handlers;

public class CreatePublisherCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreatePublisher_ThenReturnPublisherResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IPublisherRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        var mapperMock = new Mock<IMapper>();
        const string publisherName = "Test";

        userServiceMock.Setup(u => u.UserId).Returns(1);

        mapperMock.Setup(m => m.Map<PublisherResponseDto>(It.IsAny<Domain.Entities.Publisher>()))
            .Returns((Domain.Entities.Publisher b) => new PublisherResponseDto
            {
                Name = publisherName
            });

        var handler = new CreatePublisherCommandHandler(repoMock.Object, userServiceMock.Object, mapperMock.Object);

        var command = new CreatePublisherCommand(publisherName);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(publisherName, result.Name);
    }
}