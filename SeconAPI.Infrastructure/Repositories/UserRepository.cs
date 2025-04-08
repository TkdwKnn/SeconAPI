using Dapper;
using SeconAPI.Application.Interfaces.Repositories;
using SeconAPI.Domain.Entities;
using SeconAPI.Infrastructure.Data;
namespace SeconAPI.Infrastructure.Repositories;


public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;
    
    public UserRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreateUserAsync(User user)
    {
        var query = @"
            INSERT INTO users (username, email, password, password_salt, role, created_at, last_login)
            VALUES (@Username, @Email, @Password, @PasswordSalt, @Role, @CreatedAt, @LastLogin)
            RETURNING id";
        
        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(query, user);
        return id;
    }
    
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var query = "SELECT * FROM users WHERE username = @Username";
        using var connection = _context.CreateConnection();
        var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
        if (user == null)
        {
            Console.WriteLine($"User not found for username: {username}");
        }
        else
        {
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"PasswordSalt: {user.PasswordSalt ?? "null"}");
            Console.WriteLine($"Password: {user.Password ?? "null"}");
        }
        return user;
    }
    
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var query = "SELECT * FROM users WHERE id = @Id";
        
        using var connection = _context.CreateConnection();
        var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { Id = userId });
        return user;
    }
    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var query = "SELECT * FROM users ORDER BY created_at DESC";
        
        using var connection = _context.CreateConnection();
        var users = await connection.QueryAsync<User>(query);
        return users;
    }
    
    public async Task UpdateUserAsync(User user)
    {
        var query = @"
            UPDATE users
            SET username = @Username, email = @Email, password = @Password, 
                password_salt = @PasswordSalt, role = @Role, last_login = @LastLogin
            WHERE id = @Id";
        
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, user);
    }
    
    public async Task DeleteUserAsync(int userId)
    {
        var query = "DELETE FROM users WHERE id = @Id";
        
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { Id = userId });
    }
}