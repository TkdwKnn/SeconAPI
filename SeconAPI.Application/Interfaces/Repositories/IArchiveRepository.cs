namespace SeconAPI.Application.Interfaces.Repositories;


public interface IArchiveRepository
{
    Task<int> SaveArchiveAsync(int taskId, byte[] archiveData);
    Task<byte[]> GetArchiveByIdAsync(int archiveId);
    Task<byte[]> GetArchiveByTaskIdAsync(int taskId);
    
}
