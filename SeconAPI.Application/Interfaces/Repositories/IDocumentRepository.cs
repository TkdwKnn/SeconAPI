using SeconAPI.Domain.Entities;
namespace SeconAPI.Application.Interfaces.Repositories;
public interface IDocumentRepository
{
    Task<int> AddDocumentAsync(Document document);
    
    Task<Document> GetDocumentByIdAsync(int documentId);
    
    Task<IEnumerable<Document>> GetDocumentsByUserIdAsync(int userId);
    
    Task<IEnumerable<Document>> GetAllDocumentsAsync();
    
    Task DeleteDocumentAsync(int documentId);
}