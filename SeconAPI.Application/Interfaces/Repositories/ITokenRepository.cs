using SeconAPI.Domain.Entities;
namespace SeconAPI.Application.Interfaces.Repositories;


public interface ITokenRepository
{
    Task<int> CreateTokenAsync(UserToken token);
    
    Task<UserToken?> GetTokenByValueAsync(string tokenValue);
    
    Task RevokeTokenAsync(string tokenValue);
    
    Task RevokeAllUserTokensAsync(int userId);
}