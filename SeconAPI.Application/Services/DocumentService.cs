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
    private static readonly string? PythonServiceUri = Environment.GetEnvironmentVariable("PythonServiceUri") ?? "http://185.251.91.226:6555";
    private readonly IProcessingTaskRepository _processingTaskRepository;
    private readonly IExcelParser _excelParser;
    private readonly IArchiveRepository _archiveRepository;
    
    public DocumentService(IDocumentRepository documentRepository, IExcelParser excelParser, IProcessingTaskRepository processingTaskRepository, IArchiveRepository archiveRepository)
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

        try
        {
            var serialNumbers = await RecognizeMeterNumbersFromImagesAsync(imageDataList);

            var processedData = imageDataList.Zip(serialNumbers, (img, num) => new { Image = img, Number = num });

            int counter = 1;
            foreach (var item in processedData)
            {
                var matchedMeter = meterDataList.FirstOrDefault(m => m.MeterNumber == item.Number);
            
                string path = matchedMeter != null 
                    ? Path.Combine(BuildRelativePath(matchedMeter), matchedMeter.GetNewFileName() + ".jpg")
                    : $"{counter++}.jpg";

                processedImages.Add((item.Image, path));
            }
        }
        catch (Exception ex)
        {
            task.Status = "Failed";
            task.ErrorMessage = ex.Message;
            await _processingTaskRepository.UpdateTaskAsync(task);
            return task;
        }

        var modifiedExcels = new List<byte[]>();
        foreach (var report in excelReport)
        {
            var modifiedReport = await _excelParser.EditExcelReportAsync(
                report, 
                meterDataList.Where(m => !m.IsMatched).ToList()
            );
            modifiedExcels.Add(modifiedReport);
        }

        var excelFiles = modifiedExcels
            .Select((data, index) => (data, $"report_{DateTime.Now:yyyyMMddHHmm}_{index}.xlsx"))
            .ToList();
        
        var zipArchiveBytes = CreateZipArchive(processedImages, excelFiles);
        
        
        
        var archiveId = await _archiveRepository.SaveArchiveAsync(task.Id, zipArchiveBytes);

        task.Status = "Completed";
        task.ResultArchiveFileName = archiveId.ToString();
        await _processingTaskRepository.UpdateTaskAsync(task);

        return task;
    }
    
    
    
    
    private async Task<List<string>> RecognizeMeterNumbersFromImagesAsync(List<byte[]> images)
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(300);
        var content = new MultipartFormDataContent();

        for (int i = 0; i < images.Count; i++)
        {
            var imageContent = new ByteArrayContent(images[i]);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            content.Add(imageContent, "images", $"image_{i}.jpg");
        }

        var response = await client.PostAsync($"{PythonServiceUri}/batch-process", content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<string>>(jsonResponse);
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

    private static byte[] CreateZipArchive(
        List<(byte[] imageData, string path)> processedImages, 
        List<(byte[] excelData, string fileName)> excelFiles)
    {
        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var (excelData, fileName) in excelFiles)
            {
                var entry = archive.CreateEntry($"reports/{fileName}", CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                entryStream.Write(excelData, 0, excelData.Length);
            }

            foreach (var (imageData, path) in processedImages)
            {
                var entry = archive.CreateEntry(path, CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                entryStream.Write(imageData, 0, imageData.Length);
            }
        }
        return memoryStream.ToArray();
    }


    public async Task<IEnumerable<ProcessingTask>> GetProcessingTasksByUserIdAsync(int userId)
    {
        return await _processingTaskRepository.GetTasksByUserIdAsync(userId);
    }




    public async Task<byte[]> DownloadArchiveByIdAsync(int archiveId)
    {
        return await _archiveRepository.GetArchiveByIdAsync(archiveId);
    }

    public async Task<byte[]> DownloadArchiveByTaskIdAsync(int taskId)
    {
        return await _archiveRepository.GetArchiveByTaskIdAsync(taskId);
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        await _processingTaskRepository.DeleteTaskAsync(taskId);
    }
    
    
    
    
    
}