using System.Security.Cryptography;
using System.Text;
using SeconAPI.Application.Interfaces.Repositories;
using SeconAPI.Application.Interfaces.Services;
using SeconAPI.Domain.Entities;
namespace SeconAPI.Application.Services;


public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    
    public AuthService(IUserRepository userRepository, ITokenRepository tokenRepository)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
    }
    
    public async Task<UserToken> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        
        if (user == null)
        {
            throw new Exception("Invalid username or password");
        }
        
        var hashedPassword = HashPassword(password, user.PasswordSalt);
        if (hashedPassword != user.Password)
        {
            Console.WriteLine(hashedPassword);
            Console.WriteLine(user.Password);
            
            throw new Exception("Invalid username or password");
        }
        
        user.LastLogin = DateTime.UtcNow;
        await _userRepository.UpdateUserAsync(user);
        
        var token = new UserToken
        {
            UserId = user.Id,
            Token = GenerateToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };
        
        await _tokenRepository.CreateTokenAsync(token);
        return token;
    }
    
    public async Task LogoutAsync(string token)
    {
        await _tokenRepository.RevokeTokenAsync(token);
    }
    
    public async Task<User?> GetUserFromTokenAsync(string token)
    {
        var userToken = await _tokenRepository.GetTokenByValueAsync(token);
        if (userToken == null || userToken.IsRevoked || userToken.ExpiresAt < DateTime.UtcNow)
        {
            return null;
        }
        
        return await _userRepository.GetUserByIdAsync(userToken.UserId);
    }
    
    public async Task<bool> ValidateTokenAsync(string token)
    {
        var userToken = await _tokenRepository.GetTokenByValueAsync(token);
        return userToken != null && !userToken.IsRevoked && userToken.ExpiresAt >= DateTime.UtcNow;
    }
    
    private string HashPassword(string password, string salt)
    {
        Console.WriteLine(password);
        Console.WriteLine(salt);
        
        using var sha256 = SHA256.Create();
        var passwordWithSalt = password + salt;
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt));
        return Convert.ToBase64String(bytes);
    }
    
    private string GenerateToken()
    {
        return Guid.NewGuid().ToString("N");
    }
}