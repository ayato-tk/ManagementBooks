using AutoMapper;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Books.Handlers;

public class DeleteBookHandler(IBookRepository bookRepository, ICurrentUserService currentUserService, IMapper mapper)
    : IRequestHandler<DeleteBookCommand, BookResponseDto>
{
    public async Task<BookResponseDto> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var book = await bookRepository.GetByIdAsync(request.Id, query => query.Where(b => b.UserId == userId));
        if (book == null)
            throw new KeyNotFoundException("Livro não encontrado");

        await bookRepository.DeleteAsync(book);
        await bookRepository.SaveChangesAsync();
        return mapper.Map<BookResponseDto>(book);
    }
}