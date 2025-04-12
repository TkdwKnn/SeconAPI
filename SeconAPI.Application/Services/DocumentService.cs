using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SeconAPI.Application.Interfaces.Repositories;
using SeconAPI.Application.Interfaces.Services;
using SeconAPI.Domain.Entities;
namespace SeconAPI.Application.Services;


public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private static readonly string? PythonServiceUri = Environment.GetEnvironmentVariable("PythonServiceUri") ?? "http://127.0.0.1:5000";
    private readonly IProcessingTaskRepository _processingTaskRepository;
    private readonly IExcelParser _excelParser;
    private readonly IArchiveRepository _archiveRepository;
    
    public DocumentService(IDocumentRepository documentRepository, IExcelParser excelParser, IProcessingTaskRepository processingTaskRepository, IStorageService storageService, IArchiveRepository archiveRepository)
    {
        _documentRepository = documentRepository;
        _excelParser = excelParser;
        _processingTaskRepository = processingTaskRepository;
        _archiveRepository = archiveRepository;
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

    
    
   
    public async Task<ProcessingTask> ProcessReportAsync(int userId, List<byte[]> excelReport, List<byte[]> imageDataList)
    {
        var task = new ProcessingTask
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Status = "Pending"
        };

        task.Id = await _processingTaskRepository.CreateTaskAsync(task);

        var meterDataList = await _excelParser.GetMeterDataByDocumentAsync(excelReport);
        var processedImages = new List<(byte[] imageData, string newPath)>();
        int unmatchedCounter = 1;

        foreach (var imageData in imageDataList)
        {
            try
            {
                var serialNumber = await RecognizeMeterNumberFromImageAsync(imageData);
                var matchedMeter = meterDataList.FirstOrDefault(m => m.MeterNumber == serialNumber);

                if (matchedMeter != null)
                {
                    Console.WriteLine($"Found meter number {matchedMeter.MeterNumber}");
                    matchedMeter.IsMatched = true;
                    var relativePath = BuildRelativePath(matchedMeter);
                    var newFileName = matchedMeter.GetNewFileName() + ".jpg";
                    var fullPath = Path.Combine(relativePath, newFileName);
                    processedImages.Add((imageData, fullPath));
                }
                else
                {
                    var newFileName = $"{unmatchedCounter}.jpg";
                    processedImages.Add((imageData, newFileName));
                    unmatchedCounter++;
                }

            }
            catch (Exception ex)
            {
                var errorFileName = $"error_{unmatchedCounter}.jpg";
                processedImages.Add((imageData, errorFileName));
                unmatchedCounter++;
                Console.WriteLine($"Error processing image: {ex.Message}");
            }
        }

        var zipArchiveBytes = CreateZipArchive(processedImages);

        var zipPath = await _archiveRepository.SaveArchiveAsync(task.Id, zipArchiveBytes);

        task.Status = "Completed";
        task.ResultArchiveFileName = zipPath.ToString();
        await _processingTaskRepository.UpdateTaskAsync(task);

        return task;
    }
    
    
    

    private async Task<string> RecognizeMeterNumberFromImageAsync(byte[] imageData)
    {
        using var httpClient = new HttpClient();
        string base64Image = Convert.ToBase64String(imageData);

        var jsonRequest = new { image = base64Image };
        var jsonContent = JsonSerializer.Serialize(jsonRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync($"{PythonServiceUri}/recognize", content);
        response.EnsureSuccessStatusCode();

    
        return await response.Content.ReadAsStringAsync();
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
    
    
    private static string BuildRelativePath(MeterData meterData)
    {
        var pathParts = new List<string>();
    
        if (!string.IsNullOrEmpty(meterData.City))
            pathParts.Add(meterData.City);
        
        if (!string.IsNullOrEmpty(meterData.Street))
            pathParts.Add(meterData.Street);
        
        if (!string.IsNullOrEmpty(meterData.Building))
            pathParts.Add(meterData.Building);
        
        if (!string.IsNullOrEmpty(meterData.Apartment))
            pathParts.Add(meterData.Apartment);
        
        return Path.Combine(pathParts.ToArray());
    }

    private static byte[] CreateZipArchive(List<(byte[] imageData, string path)> processedImages)
    {
        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var (imageData, path) in processedImages)
            {
                var entry = archive.CreateEntry(path, CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                entryStream.Write(imageData, 0, imageData.Length);
            }
        }
        
        return memoryStream.ToArray();
    }
    
    
    public async Task<byte[]> DownloadArchiveByIdAsync(int archiveId)
    {
        return await _archiveRepository.GetArchiveByIdAsync(archiveId);
    }

    public async Task<byte[]> DownloadArchiveByTaskIdAsync(int taskId)
    {
        return await _archiveRepository.GetArchiveByTaskIdAsync(taskId);
    }
    
    
}