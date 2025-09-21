using AutoMapper;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Books.Handlers;

public class UpdateBookCommandHandler(IBookRepository bookRepository, ICurrentUserService currentUserService, IMapper mapper)
    : IRequestHandler<UpdateBookCommand, BookResponseDto>
{
    public async Task<BookResponseDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var book = await bookRepository.GetByIdAsync(request.Id, query => query.Where(b => b.UserId == userId));
        if (book == null)
            throw new KeyNotFoundException("Livro não encontrado");
        
        book.Title = request.Title ?? book.Title;
        book.Author = request.Author ?? book.Author;
        book.ISBN = request.Isbn ?? book.ISBN;
        book.Synopsis = request.Synopsis ?? book.Synopsis;
        book.CoverImagePath = request.CoverImagePath ?? book.CoverImagePath;
        book.PublisherId = request.PublisherId ?? book.PublisherId;
        book.GenreId = request.GenreId ?? book.GenreId;
        
        await bookRepository.UpdateAsync(book);
        await bookRepository.SaveChangesAsync();
        return mapper.Map<BookResponseDto>(book);
    }
}