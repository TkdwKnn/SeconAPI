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
    private static readonly string? PythonServiceUri = Environment.GetEnvironmentVariable("PythonServiceUri") ?? "http://127.0.0.1:5000/process";
    private readonly IProcessingTaskRepository _processingTaskRepository;
    private readonly IExcelParser _excelParser;
    private readonly IStorageService _storageService;
    
    public DocumentService(IDocumentRepository documentRepository, IExcelParser excelParser, IProcessingTaskRepository processingTaskRepository, IStorageService storageService)
    {
        _documentRepository = documentRepository;
        _excelParser = excelParser;
        _processingTaskRepository = processingTaskRepository;
        _storageService = storageService;
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
            Status  = "Pending"
        };
        
        task.Id = await _processingTaskRepository.CreateTaskAsync(task);

        var meterDataList = await _excelParser.GetMeterDataByDocumentAsync(excelReport);
    
        var processedImages = new List<(byte[] imageData, string newPath)>();
    
        var recognitionTasks =
            imageDataList.Select(imageData => RecognizeMeterNumberFromImageAsync(imageData)).ToList();

        
        var serialNumbers = await Task.WhenAll(recognitionTasks);
    
        
        for (var i = 0; i < imageDataList.Count; i++)
        {
            var serialNumber = serialNumbers[i];
            var matchedMeter = meterDataList.FirstOrDefault(m => m.MeterNumber == serialNumber);
        
            if (matchedMeter != null)
            {
                matchedMeter.IsMatched = true;
            
                var relativePath = BuildRelativePath(matchedMeter);
                var newFileName = matchedMeter.GetNewFileName() + ".jpg"; 
                var fullPath = Path.Combine(relativePath, newFileName);
            
                processedImages.Add((imageDataList[i], fullPath));
            }
        }
    
        var zipArchiveBytes = CreateZipArchive(processedImages);
        
        
        
        Stream stream = new MemoryStream(zipArchiveBytes);

        var zipPath = await _storageService.UploadFileAsync("secon-api", task.CreatedAt + "zip", stream, "zip" );
        
        task.Status = "Completed";
        
        task.ResultArchiveFileName =  zipPath;

        return task;
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
    
    
    private async Task<string> RecognizeMeterNumberFromImageAsync(byte[] imageData)
    {
        using var httpClient = new HttpClient();
        string base64Image = Convert.ToBase64String(imageData);
        
        var jsonRequest = new
        {
            image = base64Image
        };

        var jsonContent = JsonSerializer.Serialize(jsonRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync("http://127.0.0.1:5000/recognize", content);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }
    
    
    
    
}