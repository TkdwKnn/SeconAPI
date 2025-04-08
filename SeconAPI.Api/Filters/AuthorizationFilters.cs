using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SeconAPI.Application.Interfaces.Services;
namespace SeconAPI.Api.Filters;

public class UserAuthorizeAttribute : TypeFilterAttribute
{
    public UserAuthorizeAttribute() : base(typeof(UserAuthorizeFilter))
    {
    }
}
public class AdminAuthorizeAttribute : TypeFilterAttribute
{
    public AdminAuthorizeAttribute() : base(typeof(AdminAuthorizeFilter))
    {
    }
}



public class UserAuthorizeFilter : IAsyncAuthorizationFilter
{
    private readonly IAuthService _authService;
    
    public UserAuthorizeFilter(IAuthService authService)
    {
        _authService = authService;
    }
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var isValid = await _authService.ValidateTokenAsync(token);
        if (!isValid)
        {
            context.Result = new UnauthorizedResult();
        }
        
    }
}


public class AdminAuthorizeFilter : IAsyncAuthorizationFilter
{
    private readonly IAuthService _authService;
    
    public AdminAuthorizeFilter(IAuthService authService)
    {
        _authService = authService;
    }
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var isValid = await _authService.ValidateTokenAsync(token);

        if (!isValid)
        {
            context.Result = new UnauthorizedResult();
            return; 
        }

        var user = await _authService.GetUserFromTokenAsync(token);

        if (user is not { Role: "Admin" }) 
        {
            context.Result = new ForbidResult();
        }
    }
}