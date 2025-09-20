using GestaoLivros.Domain.Entities;

namespace GestaoLivros.Application.Services.Interfaces;

public interface IReportService
{
    byte[] GenerateBooksReport(List<Book> books);
}