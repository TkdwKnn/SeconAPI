using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeconAPI.Api.Contracts;
using SeconAPI.Api.Filters;
using SeconAPI.Application.Interfaces.Services;
namespace SeconAPI.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly IAuthService _authService;
    private readonly IExcelParser _excelParser;
    
    
    public DocumentsController(IDocumentService documentService, IAuthService authService, IExcelParser excelParser)
    {
        _documentService = documentService;
        _authService = authService;
        _excelParser = excelParser;
    }
    
    [HttpPost("upload")]
    [UserAuthorize]
    public async Task<IActionResult> UploadDocument(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file was uploaded" });
            }
            
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedTypes.Contains(file.ContentType))
            {
                return BadRequest(new { message = "Only JPEG and PNG images are supported" });
            }
            
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _authService.GetUserFromTokenAsync(token);
            
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var documentId = await _documentService.ProcessImageAsync(memoryStream.ToArray(), user.Id);
            
            return Ok(new { documentId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    [UserAuthorize]
    public async Task<IActionResult> GetDocumentById(int id)
    {
        try
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _authService.GetUserFromTokenAsync(token);
            
            if (user.Role != "Admin" && document.UserId != user.Id)
            {
                return Forbid();
            }
            
            return Ok(new DocumentResponse(document));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("user")]
    [UserAuthorize]
    public async Task<IActionResult> GetUserDocuments()
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _authService.GetUserFromTokenAsync(token);
            
            var documents = await _documentService.GetDocumentsByUserIdAsync(user.Id);
            return Ok(DocumentResponse.FromDocumentList(documents));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet]
    [AdminAuthorize]
    public async Task<IActionResult> GetAllDocuments()
    {
        try
        {
            var documents = await _documentService.GetAllDocumentsAsync();
            return Ok(DocumentResponse.FromDocumentList(documents));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    [UserAuthorize]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        try
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await _authService.GetUserFromTokenAsync(token);
            
            if (user.Role != "Admin" && document.UserId != user.Id)
            {
                return Forbid();
            }
            
            await _documentService.DeleteDocumentAsync(id);
            return Ok(new { message = "Document deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadFiles()
    {
        var files = Request.Form.Files;
        var excelFiles = files.Where(f => f.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        var images = files.Where(f => f.ContentType.StartsWith("image/")).ToList();

        

        

        var imageDataList = new List<byte[]>();
        foreach (var image in excelFiles)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                imageDataList.Add(memoryStream.ToArray());
            }
        }

        var result = await _excelParser.GetMeterDataByDocumentAsync(imageDataList);

        return Ok(result);
    }
    
    
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
    [DisableRequestSizeLimit]
    [HttpPost("process")]
    [UserAuthorize]
    public async Task<IActionResult> ProcessTask()
    {
        var files = Request.Form.Files;
        var excelFiles = files.Where(f => f.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        var images = files.Where(f => f.ContentType.StartsWith("image/")).ToList();

        if (excelFiles == null)
        {
            return BadRequest("Excel-файл обязателен.");
        }

        var excelData = new List<byte[]>();

        foreach (var report in excelFiles)
        {
            using var memoryStream = new MemoryStream();
            await report.CopyToAsync(memoryStream);
            excelData.Add(memoryStream.ToArray());
        }

        var imageDataList = new List<byte[]>();
        foreach (var image in images)
        {
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            imageDataList.Add(memoryStream.ToArray());
        }

        
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var user = await _authService.GetUserFromTokenAsync(token);
        
        var result = await _documentService.ProcessReportAsync(user.Id, excelData, imageDataList);
        
        return Ok(result);
    }
    
    [HttpGet("archive/{archiveId}")]
    public async Task<IActionResult> DownloadArchiveById(int archiveId)
    {
        try
        {
            var archiveBytes = await _documentService.DownloadArchiveByIdAsync(archiveId);
            
            if (archiveBytes == null || archiveBytes.Length == 0)
                return NotFound("Archive not found");

            return File(archiveBytes, "application/zip", $"archive_{archiveId}.zip");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    
    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> DownloadArchiveByTaskId(int taskId)
    {
        try
        {
            var archiveBytes = await _documentService.DownloadArchiveByTaskIdAsync(taskId);
            
            if (archiveBytes == null || archiveBytes.Length == 0)
                return NotFound("Archive not found for this task");

            return File(archiveBytes, "application/zip", $"task_{taskId}_archive.zip");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet("proccessed/{userId}")]
    [AdminAuthorize]
    public async Task<IActionResult> ProcessTaskByUserId(int userId)
    {
        var processes = await _documentService.GetProcessingTasksByUserIdAsync(userId);
        return Ok(processes);
    }
    
    
    [HttpGet("proccessed/my")]
    [AdminAuthorize]
    public async Task<IActionResult> ProcessTaskForUser()
    {
        
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var user = await _authService.GetUserFromTokenAsync(token);
        
        var processes = await _documentService.GetProcessingTasksByUserIdAsync(user.Id);
        return Ok(processes);
    }



}