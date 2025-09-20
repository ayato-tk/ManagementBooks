namespace GestaoLivros.Application.Requests.Books.DTOs;

public class BookResponseDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;
    
    public string Isbn { get; init; } = string.Empty;
    
    public string Author { get; init; } = string.Empty;
    
    public string Synopsis { get; init; } = string.Empty;

    public string CoverImagePath { get; init; } = string.Empty;
    
    public int PublisherId { get; init; }
    
    public int GenreId { get; init; }
}