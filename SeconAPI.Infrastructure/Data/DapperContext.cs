using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace SeconAPI.Infrastructure.Data;

public class DapperContext
{
    
    private readonly string? _connectionString;
    
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }


    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}