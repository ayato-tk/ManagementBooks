using GestaoLivros.Application.Services.Interfaces;
using QuestPDF.Fluent;
using GestaoLivros.Domain.Entities;

namespace GestaoLivros.Infra.Data.Reports;

public class ReportService : IReportService
{
    public byte[] GenerateBooksReport(List<Book> books)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Header()
                    .Text($"Relatório de Livros")
                    .FontSize(18).Bold().AlignCenter();

                page.Content().PaddingTop(20).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Título").Bold();
                        header.Cell().Text("Autor").Bold();
                        header.Cell().Text("ISBN").Bold();
                        header.Cell().Text("Gênero").Bold();
                        header.Cell().Text("Publicadora").Bold();
                    });

                    foreach (var book in books)
                    {
                        table.Cell().Text(book.Title ?? "-");
                        table.Cell().Text(book.Author ?? "-");
                        table.Cell().Text(book.ISBN ?? "-");
                        table.Cell().Text(book.Genre?.Name ?? "-");
                        table.Cell().Text(book.Publisher?.Name ?? "-");
                    }
                });

                page.Footer()
                    .AlignRight()
                    .Text(text =>
                    {
                        text.Span("Gerado em: ").Bold();
                        text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    });
            });
        });

        return document.GeneratePdf();
    }
}