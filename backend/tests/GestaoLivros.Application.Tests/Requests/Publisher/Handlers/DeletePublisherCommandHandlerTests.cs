using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.Commands;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Requests.Publisher.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Publisher.Handlers;

public class DeletePublisherCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteBook_ReturnPublisherResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IPublisherRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        userServiceMock.Setup(u => u.UserId).Returns(1);

        var mapperMock = new Mock<IMapper>();
        mapperMock
            .Setup(m => m.Map<PublisherResponseDto>(It.IsAny<Domain.Entities.Publisher>()))
            .Returns((Domain.Entities.Publisher b) => new PublisherResponseDto { Name = b.Name! });

        var publisher = new Domain.Entities.Publisher { Id = 1, Name = "Teste" };

        repoMock.Setup(r => r.GetByIdAsync(publisher.Id,
                It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
            .ReturnsAsync(publisher);

        var command = new DeletePublisherCommand
        {
            Id = 1
        };

        var handler = new DeletePublisherCommandHandler(
            repoMock.Object,
            userServiceMock.Object,
            mapperMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(command.Name, result.Name);
    }
}