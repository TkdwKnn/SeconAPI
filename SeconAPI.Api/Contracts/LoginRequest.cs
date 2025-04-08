using System.ComponentModel.DataAnnotations;

namespace SeconAPI.Api.Contracts;

public class LoginRequest
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    public required string Password { get; set; }
}