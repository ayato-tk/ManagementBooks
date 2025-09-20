namespace GestaoLivros.Domain.Entities;

public class PasswordResetToken
{
    public int Id { get; set; }
    
    public long UserId { get; set; }
    
    public string Token { get; set; } = null!;
    
    public DateTime Expiration { get; set; }
    
    public bool Used { get; set; } = false;

    public virtual User User { get; set; } = null!;
}