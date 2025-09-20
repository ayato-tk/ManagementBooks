namespace GestaoLivros.Application.Requests.Books.DTOs;

public abstract class CreateBookRequestDto
{
    
    public string Title { get; init; } = string.Empty;

    public string Isbn { get; init; } = string.Empty;

    public string Author { get; init; } = string.Empty;
    
    public string Synopsis { get; init; } = string.Empty;

    public string? CoverImagePath { get; init; }

    public int PublisherId { get; init; }
    
    public int GenreId { get; init; }
}