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
    
    public DocumentsController(IDocumentService documentService, IAuthService authService)
    {
        _documentService = documentService;
        _authService = authService;
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
}