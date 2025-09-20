using System.ComponentModel.DataAnnotations;
using GestaoLivros.Domain.Exceptions;

namespace GestaoLivros.Domain.Entities;

public class Book
{
    public int Id { get; init; }
    
    [Required]
    public string? Title { get; set; }
    
    [Required]
    public string? ISBN { get; set; }

    [Required] 
    public string Author { get; set; } = string.Empty;

    public string? Synopsis { get; set; } = string.Empty;

    public string? CoverImagePath { get; set; }
    
    public int PublisherId { get; set; }
    public Publisher? Publisher { get; init; }

    public int GenreId { get; set; }
    public Genre? Genre { get; init; }
    
    public long UserId { get; init; }
    public User? User { get; init; }
    
    public Book() {}

    public Book(
        string? title,
        string? isbn, 
        string? author,
        string? synopsis,
        int? genreId, 
        int? publisherId,
        long userId
        )
    {
        if (string.IsNullOrWhiteSpace(title)) throw new DomainException("Título inválido");
        if (string.IsNullOrWhiteSpace(isbn)) throw new DomainException("ISBN inválido");
        if (isbn.Length is < 10 or > 13) throw new DomainException("ISBN deve ter 10 a 13 caracteres");
        if (genreId <= 0) throw new DomainException("Gênero é obrigatório");
        if (publisherId <= 0) throw new DomainException("Publicadora é obrigatório");

        Title = title;
        ISBN = isbn;
        Author = author ?? string.Empty;
        GenreId = genreId ?? 0;
        Synopsis = synopsis;
        PublisherId = publisherId ?? 0;
        UserId = userId;
    }
}