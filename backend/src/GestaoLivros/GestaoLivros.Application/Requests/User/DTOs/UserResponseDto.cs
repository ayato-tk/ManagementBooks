namespace GestaoLivros.Application.Requests.User.DTOs;

public class UserResponseDto
{
    public long Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }
}