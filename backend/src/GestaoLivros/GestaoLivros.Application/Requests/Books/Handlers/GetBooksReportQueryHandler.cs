using GestaoLivros.Application.Requests.Books.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestaoLivros.Application.Requests.Books.Handlers;

public class GetBooksReportQueryHandler(
    IBookRepository bookRepository,
    ICurrentUserService currentUserService,
    IReportService reportService)
    : IRequestHandler<GetBooksReportQuery, byte[]>
{
    public async Task<byte[]> Handle(GetBooksReportQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var books = await bookRepository.GetAllAsync(q =>
            q.Where(b => b.UserId == userId)
                .Include(b => b.Genre)
                .Include(b => b.Publisher));
        return reportService.GenerateBooksReport(books);
    }
}