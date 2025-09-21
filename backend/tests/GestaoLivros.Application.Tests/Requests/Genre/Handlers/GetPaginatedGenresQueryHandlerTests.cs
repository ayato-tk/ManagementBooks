using System.Linq.Expressions;
using AutoMapper;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Requests.Genre.Handlers;
using GestaoLivros.Application.Requests.Genre.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Genre.Handlers;

public class GetPaginatedGenresQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldGetPaginatedGenres_ReturnPaginatedGenreResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IGenreRepository>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<ICurrentUserService>();
        userServiceMock.Setup(u => u.UserId).Returns(1);

        var genres = new List<Domain.Entities.Genre>
        {
            new() { Id = 1, Name = "Teste" },
            new() { Id = 2, Name = "Teste2" }
        };

        repoMock.Setup(r => r.GetPaginatedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>(),
                It.IsAny<Expression<Func<Domain.Entities.Genre, string>>[]>()
            ))
            .ReturnsAsync((genres, genres.Count));

        mapperMock.Setup(m => m.Map<List<GenreResponseDto>>(genres))
            .Returns(genres.Select(b => new GenreResponseDto() { Name = b.Name! }).ToList());

        var handler = new GetPaginatedGenresQueryHandler(userServiceMock.Object, repoMock.Object, mapperMock.Object);
        var query = new GetPaginatedGenresQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.TotalItems);
        Assert.Equal("Teste", result.Data[0].Name);
        Assert.Equal("Teste2", result.Data[1].Name);
    }
}