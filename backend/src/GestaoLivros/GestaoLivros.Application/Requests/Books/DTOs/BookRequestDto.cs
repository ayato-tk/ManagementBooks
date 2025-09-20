namespace GestaoLivros.Application.Requests.Books.DTOs;

public abstract class BookRequestDto
{
    public int Id { get; init; }
    
    public string? Title { get; init; }

    public string? Isbn { get; init; }

    public string? Author { get; init; }
    
    public string? Synopsis { get; init; }

    public string? CoverImagePath { get; init; }

    public int? PublisherId { get; init; }
    
    public int? GenreId { get; init; }
}