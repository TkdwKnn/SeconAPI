using SeconAPI.Domain.Entities;

namespace SeconAPI.Application.Interfaces.Repositories;

public interface IProcessingTaskRepository
{
    Task<int> CreateTaskAsync(ProcessingTask task);
    Task<ProcessingTask?> GetTaskByIdAsync(int taskId);
    Task<IEnumerable<ProcessingTask>> GetTasksByUserIdAsync(int userId);
    Task UpdateTaskAsync(ProcessingTask task);
    Task DeleteTaskAsync(int taskId);
}