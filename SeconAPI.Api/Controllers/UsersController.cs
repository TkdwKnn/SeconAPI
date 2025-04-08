using Microsoft.AspNetCore.Mvc;
using SeconAPI.Api.Filters;
using SeconAPI.Application.Interfaces.Services;
using SeconAPI.Api.Contracts;
namespace SeconAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    
    public UsersController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }
    
    [HttpPost]
    [AdminAuthorize]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var userId = await _userService.CreateUserAsync(
                request.Username, 
                request.Email, 
                request.Password, 
                request.Role ?? "User"
            );
            
            return Ok(new { userId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet]
    [AdminAuthorize]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = users.Select(u => new {
                id = u.Id,
                username = u.Username,
                email = u.Email,
                role = u.Role,
                createdAt = u.CreatedAt,
                lastLogin = u.LastLogin
            });
            
            return Ok(userDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    [UserAuthorize]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var currentUser = await _authService.GetUserFromTokenAsync(token);
            
            if (currentUser.Role != "Admin" && currentUser.Id != id)
            {
                return Forbid();
            }
            
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(new {
                id = user.Id,
                username = user.Username,
                email = user.Email,
                role = user.Role,
                createdAt = user.CreatedAt,
                lastLogin = user.LastLogin
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    [AdminAuthorize]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new { message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}