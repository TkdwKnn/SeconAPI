using System.Data;
using Dapper;
using SeconAPI.Application.Interfaces.Repositories;
using SeconAPI.Infrastructure.Data;

namespace SeconAPI.Infrastructure.Repositories;

public class ArchiveRepository : IArchiveRepository
{
    private readonly DapperContext _context;

    public ArchiveRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> SaveArchiveAsync(int taskId, byte[] archiveData)
    {
        const string sql = @"
            INSERT INTO archive_files (task_id, archive_data)
            VALUES (@TaskId, @ArchiveData)
            RETURNING id";

        using var connection = _context.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("TaskId", taskId);
        parameters.Add("ArchiveData", archiveData, DbType.Binary);

        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }

    public async Task<byte[]> GetArchiveByIdAsync(int archiveId)
    {
        const string sql = "SELECT archive_data FROM archive_files WHERE id = @ArchiveId";
        
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<byte[]>(sql, new { ArchiveId = archiveId });
    }

    public async Task<byte[]> GetArchiveByTaskIdAsync(int taskId)
    {
        const string sql = "SELECT archive_data FROM archive_files WHERE task_id = @TaskId ORDER BY created_at DESC LIMIT 1";
        
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<byte[]>(sql, new { TaskId = taskId });
    }

    
}