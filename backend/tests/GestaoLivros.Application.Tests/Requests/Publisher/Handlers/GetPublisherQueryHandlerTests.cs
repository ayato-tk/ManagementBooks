using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Requests.Publisher.Handlers;
using GestaoLivros.Application.Requests.Publisher.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Publisher.Handlers;

public class GetPublisherQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldGetPublisherById_ReturnPublisherResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IPublisherRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        userServiceMock.Setup(u => u.UserId).Returns(1);

        var mapperMock = new Mock<IMapper>();
        mapperMock
            .Setup(m => m.Map<PublisherResponseDto>(It.IsAny<Domain.Entities.Publisher>()))
            .Returns((Domain.Entities.Publisher b) => new PublisherResponseDto() { Id = b.Id, Name = b.Name! });

        var publisher = new Domain.Entities.Publisher { Id = 1, Name = "Teste" };

        repoMock.Setup(r => r.GetByIdAsync(publisher.Id,
                It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
            .ReturnsAsync(publisher);

        var command = new GetPublisherQuery(1);

        var handler = new GetPublisherQueryHandler(
            userServiceMock.Object,
            repoMock.Object,
            mapperMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.Id);
    }
}