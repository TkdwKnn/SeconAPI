using Dapper;
using SeconAPI.Application.Interfaces.Repositories;
using SeconAPI.Domain.Entities;
using SeconAPI.Infrastructure.Data;
namespace SeconAPI.Infrastructure.Repositories;



public class DocumentRepository : IDocumentRepository
{
    private readonly DapperContext _context;
    
    public DocumentRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<int> AddDocumentAsync(Document document)
    {
        var query = @"
                    INSERT INTO documents (user_id, date, fields, file_name, content_type, status)
                    VALUES (@UserId, @Date, @Fields::jsonb, @FileName, @ContentType, @Status)
                    RETURNING id";
                        
        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(query, document);
        return id;
    }
    
    public async Task<Document> GetDocumentByIdAsync(int documentId)
    {
        var query = "SELECT * FROM documents WHERE id = @Id";
        
        using var connection = _context.CreateConnection();
        var document = await connection.QuerySingleOrDefaultAsync<Document>(query, new { Id = documentId });

        if (document == null)
        {
            throw new Exception($"Document with id {documentId} not found");
        }

        return document;
    }
    
    public async Task<IEnumerable<Document>> GetDocumentsByUserIdAsync(int userId)
    {
        var query = "SELECT * FROM documents WHERE user_id = @UserId ORDER BY date DESC";
        
        using var connection = _context.CreateConnection();
        var documents = await connection.QueryAsync<Document>(query, new { UserId = userId });
        return documents;
    }
    
    public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
    {
        var query = "SELECT * FROM documents ORDER BY date DESC";
        
        using var connection = _context.CreateConnection();
        var documents = await connection.QueryAsync<Document>(query);
        return documents;
    }
    
    public async Task DeleteDocumentAsync(int documentId)
    {
        var query = "DELETE FROM documents WHERE id = @Id";
        
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { Id = documentId });
    }
}