using GestaoLivros.Domain.Entities;
using GestaoLivros.Infra.Data.Reports;
using QuestPDF.Infrastructure;

namespace GestaoLivros.Infra.Data.Tests.Reports;

public class ReportServiceTests
{
    [Fact]
    public void GenerateBooksReport_ShouldNotThrow()
    {
        // Arrange
        QuestPDF.Settings.License = LicenseType.Community;
        var service = new ReportService();
        var books = new List<Book>
        {
            new("Livro1", "1234567890", "Autor1", "Sinopse", 1, 1, 1)
            {
                Genre = new Genre { Name = "Fantasia" },
                Publisher = new Publisher { Name = "Editora1" }
            }
        };

        // Act
        var pdfBytes = service.GenerateBooksReport(books);

        // Assert
        Assert.NotNull(pdfBytes);
        Assert.NotEmpty(pdfBytes);
    }
}