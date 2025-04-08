
using System.Text.Json;
using SeconAPI.Domain.Entities;

namespace SeconAPI.Api.Contracts;

public class DocumentResponse(Document document)
{
    public int Id { get; set; } = document.Id;
    public int UserId { get; set; } = document.UserId;
    public DateTime Date { get; set; } = document.Date;
    public JsonDocument Fields { get; set; } = JsonDocument.Parse(document.Fields);
    public string? FileName { get; set; } = document.FileName;
    public string? ContentType { get; set; } = document.ContentType;
    public string Status { get; set; } = document.Status;
    
    public static List<DocumentResponse> FromDocumentList(IEnumerable<Document> documents)
    {
        return documents.Select(doc => new DocumentResponse(doc)).ToList();
    }
    
}