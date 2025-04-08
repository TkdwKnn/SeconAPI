using System.ComponentModel.DataAnnotations;
namespace SeconAPI.Api.Contracts;

public class CreateUserRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string Username { get; set; }
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }
    
    public string? Role { get; set; }
}