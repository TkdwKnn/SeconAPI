using Dapper;
using SeconAPI.Application.Interfaces.Repositories;
using SeconAPI.Domain.Entities;
using SeconAPI.Infrastructure.Data;

namespace SeconAPI.Infrastructure.Repositories;

public class ProcessingTaskRepository : IProcessingTaskRepository
{
    private readonly DapperContext _context;
    
    public ProcessingTaskRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreateTaskAsync(ProcessingTask task)
    {
        var query = @"
            INSERT INTO processing_tasks (user_id, created_at, status, error_message, result_archive_file_name)
            VALUES (@UserId, @CreatedAt, @Status, @ErrorMessage, @ResultArchiveFileName)
            RETURNING id";
            
        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(query, task);
        return id;
    }
    
    public async Task<ProcessingTask?> GetTaskByIdAsync(int taskId)
    {
        var query = "SELECT * FROM processing_tasks WHERE id = @Id";
        using var connection = _context.CreateConnection();
        var task = await connection.QuerySingleOrDefaultAsync<ProcessingTask>(query, new { Id = taskId });
        return task;
    }
    
    public async Task<IEnumerable<ProcessingTask>> GetTasksByUserIdAsync(int userId)
    {
        var query = "SELECT * FROM processing_tasks WHERE user_id = @UserId ORDER BY created_at DESC";
        using var connection = _context.CreateConnection();
        var tasks = await connection.QueryAsync<ProcessingTask>(query, new { UserId = userId });
        return tasks;
    }
    
    public async Task UpdateTaskAsync(ProcessingTask task)
    {
        var query = @"
            UPDATE processing_tasks
            SET status = @Status, 
                error_message = @ErrorMessage, 
                result_archive_file_name = @ResultArchiveFileName
            WHERE id = @Id";
            
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, task);
    }
    
    public async Task DeleteTaskAsync(int taskId)
    {
        var query = "DELETE FROM processing_tasks WHERE id = @Id";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { Id = taskId });
    }
}