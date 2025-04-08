using SeconAPI.Application.Interfaces.Repositories;
using SeconAPI.Application.Interfaces.Services;
using SeconAPI.Domain.Entities;
namespace SeconAPI.Application.Services;


public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private static readonly string? PythonServiceUri = Environment.GetEnvironmentVariable("PythonServiceUri") ?? "http://127.0.0.1:5000/process";
    
    public DocumentService(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }
    
    public async Task<int> ProcessImageAsync(byte[] image, int userId)
    {
        using var client = new HttpClient();
        
        var content = new ByteArrayContent(image);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        
        var response = await client.PostAsync(PythonServiceUri, content);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error processing image: {response.StatusCode}");
        }
        
        var jsonFields = await response.Content.ReadAsStringAsync();
        var document = new Document
        {
            UserId = userId,
            Date = DateTime.UtcNow,
            Fields = jsonFields,
            Status = "Processed"
        };
        
        return await _documentRepository.AddDocumentAsync(document);
    }
    
    public async Task<Document> GetDocumentByIdAsync(int documentId)
    {
        return await _documentRepository.GetDocumentByIdAsync(documentId);
    }
    
    public async Task<List<Document>> GetDocumentsByUserIdAsync(int userId)
    {
        var documents = await _documentRepository.GetDocumentsByUserIdAsync(userId);
        return documents.ToList();
    }
    
    public async Task<List<Document>> GetAllDocumentsAsync()
    {
        var documents = await _documentRepository.GetAllDocumentsAsync();
        return documents.ToList();
    }
    
    public async Task DeleteDocumentAsync(int documentId)
    {
        await _documentRepository.DeleteDocumentAsync(documentId);
    }
}