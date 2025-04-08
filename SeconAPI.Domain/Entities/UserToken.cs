namespace SeconAPI.Domain.Entities;

public class UserToken
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public required string Token { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ExpiresAt { get; set; }
    
    public bool IsRevoked { get; set; } = false;
}