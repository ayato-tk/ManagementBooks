using System.Linq.Expressions;
using AutoMapper;
using GestaoLivros.Application.Requests.Publisher.DTOs;
using GestaoLivros.Application.Requests.Publisher.Handlers;
using GestaoLivros.Application.Requests.Publisher.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Publisher.Handlers;

public class GetPaginatedPublishersQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldGetPaginatedPublishers_ReturnPaginatedPublisherResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IPublisherRepository>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<ICurrentUserService>();
        userServiceMock.Setup(u => u.UserId).Returns(1);

        var publishers = new List<Domain.Entities.Publisher>
        {
            new() { Id = 1, Name = "Teste" },
            new() { Id = 2, Name = "Teste2" }
        };

        repoMock.Setup(r => r.GetPaginatedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>(),
                It.IsAny<Expression<Func<Domain.Entities.Publisher, string>>[]>()
            ))
            .ReturnsAsync((publishers, publishers.Count));

        mapperMock.Setup(m => m.Map<List<PublisherResponseDto>>(publishers))
            .Returns(publishers.Select(b => new PublisherResponseDto() { Name = b.Name! }).ToList());

        var handler =
            new GetPaginatedPublishersQueryHandler(userServiceMock.Object, repoMock.Object, mapperMock.Object);
        var query = new GetPaginatedPublishersQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.TotalItems);
        Assert.Equal("Teste", result.Data[0].Name);
        Assert.Equal("Teste2", result.Data[1].Name);
    }
}