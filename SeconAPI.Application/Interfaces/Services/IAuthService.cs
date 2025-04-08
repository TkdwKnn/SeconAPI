using SeconAPI.Domain.Entities;
namespace SeconAPI.Application.Interfaces.Services;

public interface IAuthService
{
    Task<UserToken> LoginAsync(string username, string password);
    
    Task LogoutAsync(string token);
    
    Task<User?> GetUserFromTokenAsync(string token);
    
    Task<bool> ValidateTokenAsync(string token);
}