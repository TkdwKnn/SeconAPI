using System.Security.Cryptography;
using System.Text;
using SeconAPI.Application.Interfaces.Repositories;
using SeconAPI.Application.Interfaces.Services;
using SeconAPI.Domain.Entities;
namespace SeconAPI.Application.Services;


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<int> CreateUserAsync(string username, string email, string password, string role)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(username);
        if (existingUser != null)
        {
            throw new Exception("Username already exists");
        }
        
        var salt = GenerateSalt();
        var hashedPassword = HashPassword(password, salt);
        
        var user = new User
        {
            Username = username,
            Email = email,
            Password = hashedPassword,
            PasswordSalt = salt,
            Role = role
        };
        
        return await _userRepository.CreateUserAsync(user);
    }
    
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }
    
    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.ToList();
    }
    
    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateUserAsync(user);
    }
    
    public async Task DeleteUserAsync(int userId)
    {
        await _userRepository.DeleteUserAsync(userId);
    }
    
    private string GenerateSalt()
    {
        var saltBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }
    
    private string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var passwordWithSalt = password + salt;
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt));
        return Convert.ToBase64String(bytes);
    }
}