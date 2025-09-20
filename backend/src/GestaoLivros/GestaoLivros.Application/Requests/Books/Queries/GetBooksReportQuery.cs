using MediatR;

namespace GestaoLivros.Application.Requests.Books.Queries;

public record GetBooksReportQuery() : IRequest<byte[]>;