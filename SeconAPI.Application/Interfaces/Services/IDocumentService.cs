using SeconAPI.Domain.Entities;
namespace SeconAPI.Application.Interfaces.Services;
public interface IDocumentService
{
    Task<int> ProcessImageAsync(byte[] image, int userId);
    
    Task<ProcessingTask> ProcessReportAsync(byte[] excelReport, List<byte[]> imageDataList);
    
    Task<Document> GetDocumentByIdAsync(int documentId);
    
    Task<List<Document>> GetDocumentsByUserIdAsync(int userId);
    
    Task<List<Document>> GetAllDocumentsAsync();
    
    Task DeleteDocumentAsync(int documentId);
}