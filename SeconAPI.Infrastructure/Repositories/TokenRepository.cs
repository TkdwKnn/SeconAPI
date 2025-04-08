using Dapper;
using SeconAPI.Application.Interfaces.Repositories;
using SeconAPI.Domain.Entities;
using SeconAPI.Infrastructure.Data;
namespace SeconAPI.Infrastructure.Repositories;


public class TokenRepository : ITokenRepository
{
    private readonly DapperContext _context;
    
    public TokenRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreateTokenAsync(UserToken token)
    {
        var query = @"
            INSERT INTO user_tokens (user_id, token, created_at, expires_at, is_revoked)
            VALUES (@UserId, @Token, @CreatedAt, @ExpiresAt, @IsRevoked)
            RETURNING id";
        
        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(query, token);
        return id;
    }
    
    public async Task<UserToken?> GetTokenByValueAsync(string tokenValue)
    {
        var query = "SELECT * FROM user_tokens WHERE token = @Token";
        
        using var connection = _context.CreateConnection();
        var token = await connection.QuerySingleOrDefaultAsync<UserToken>(query, new { Token = tokenValue });
        return token;
    }
    
    public async Task RevokeTokenAsync(string tokenValue)
    { 
        var query = "UPDATE user_tokens SET is_revoked = true WHERE token = @Token";
        if (query == null) throw new ArgumentNullException(nameof(query));

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { Token = tokenValue });
    }
    
    public async Task RevokeAllUserTokensAsync(int userId)
    {
        var query = "UPDATE user_tokens SET is_revoked = true WHERE user_id = @UserId";
        
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { UserId = userId });
    }
}