using SeconAPI.Domain.Entities;
namespace SeconAPI.Application.Interfaces.Services;


public interface IUserService
{
    Task<int> CreateUserAsync(string username, string email, string password, string role);
    
    Task<User?> GetUserByIdAsync(int userId);
    
    Task<List<User>> GetAllUsersAsync();
    
    Task UpdateUserAsync(User user);
    
    Task DeleteUserAsync(int userId);
}