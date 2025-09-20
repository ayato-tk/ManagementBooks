using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoLivros.Domain.Entities;

[Table("User")]
public class User
{
    public long Id { get; init; }

    public required string Name { get; init; }
    
    [EmailAddress]
    public required string Email { get; init; }

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime Birthdate { get; init; }

    public ICollection<Genre> Genre { get; init; } = new List<Genre>();
    
    public ICollection<Publisher> Publisher { get; init; } = new List<Publisher>();
    
    public ICollection<Book> Books { get; init; } = new List<Book>();
}