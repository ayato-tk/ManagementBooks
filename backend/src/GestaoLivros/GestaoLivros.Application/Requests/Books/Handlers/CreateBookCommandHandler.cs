using AutoMapper;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using MediatR;

namespace GestaoLivros.Application.Requests.Books.Handlers;

public class CreatBookHandler(IBookRepository bookRepository, ICurrentUserService currentUserService, IMapper mapper)
    : IRequestHandler<CreateBookCommand, BookResponseDto>
{
    public async Task<BookResponseDto> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var book = new Book(
            command.Title,
            command.Isbn,
            command.Author,
            command.Synopsis,
            command.GenreId,
            command.PublisherId,
            currentUserService.UserId
        )
        { 
            CoverImagePath = command.CoverImagePath
        };

        await bookRepository.AddAsync(book);
        await bookRepository.SaveChangesAsync();
        return mapper.Map<BookResponseDto>(book);
    }
}