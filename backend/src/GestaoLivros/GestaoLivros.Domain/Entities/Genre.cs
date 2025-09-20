namespace GestaoLivros.Domain.Entities;

public class Genre
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public long UserId { get; init; }
    public User? User { get; init; } 

    public ICollection<Book> Books { get; init; } = new List<Book>();
}