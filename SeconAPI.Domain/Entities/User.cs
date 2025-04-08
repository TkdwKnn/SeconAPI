using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeconAPI.Domain.Entities;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public required string Username { get; set; }
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public required string Email { get; set; }
    
    [Required]
    public required string Password { get; set; }
    
    [Required]
    public required string PasswordSalt { get; set; }
    
    [Required]
    public required string Role { get; set; } = "User";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastLogin { get; set; }
}