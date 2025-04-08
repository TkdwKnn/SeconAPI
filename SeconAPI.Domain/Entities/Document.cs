using System.ComponentModel.DataAnnotations;
namespace SeconAPI.Domain.Entities;
public class Document
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public required DateTime Date { get; set; }
    
    public required string Fields { get; set; }
    
    public string? FileName { get; set; }
    
    public string? ContentType { get; set; }
    
    public string Status { get; set; } = "Pending";
}