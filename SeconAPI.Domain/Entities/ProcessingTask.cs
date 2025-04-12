namespace SeconAPI.Domain.Entities;

public class ProcessingTask
{
    
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
    public string? ErrorMessage { get; set; }
    public string? OriginalExcelFileName { get; set; }
    public string? ResultArchiveFileName { get; set; }
    public string? WorkingDirectory { get; set; }
    
}