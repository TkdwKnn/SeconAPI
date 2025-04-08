namespace SeconAPI.Application.Interfaces.Repositories;

using SeconAPI.Domain.Entities;

public interface IUserRepository
{
    Task<int> CreateUserAsync(User user);
    
    Task<User?> GetUserByUsernameAsync(string username);
    
    Task<User?> GetUserByIdAsync(int userId);
    
    Task<IEnumerable<User>> GetAllUsersAsync();
    
    Task UpdateUserAsync(User user);
    
    Task DeleteUserAsync(int userId);
}