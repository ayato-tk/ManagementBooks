using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Requests.Publisher.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Publisher.Handlers;

public class UpdatePublisherCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdatePublisher_ReturnUpdatedPublisherResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IPublisherRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        var mapperMock = new Mock<IMapper>();

        var publishers = new Domain.Entities.Publisher { Id = 1, Name = "Teste" };

        repoMock.Setup(r => r.GetByIdAsync(publishers.Id,
                It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
            .ReturnsAsync(publishers);

        mapperMock.Setup(m => m.Map<PublisherResponseDto>(It.IsAny<Domain.Entities.Publisher>()))
            .Returns((Domain.Entities.Publisher b) => new PublisherResponseDto { Id = b.Id, Name = b.Name! });

        var handler = new UpdatePublisherCommandHandler(repoMock.Object, userServiceMock.Object, mapperMock.Object);

        var command = new UpdatePublisherCommand
        {
            Id = publishers.Id,
            Name = publishers.Name
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Teste", publishers.Name);
    }
}