using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Extensions;
using RiffBackend.API.Requests;
using RiffBackend.API.Responses;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Shared;

namespace RiffBackend.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService service, IConfiguration configuration) : Controller
{
    private readonly IUserService _service = service;
    private readonly string _coockieName = configuration["Authentication:CookieName"]
               ?? throw new InvalidOperationException("CookieName is missing!");

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        return result.ToActionResult(user => Ok(new UserResponse(user.Id, user.Name, user.AvatarUrl)));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] UserRequest request)
    {
        Stream? stream = null;
        string fileName = "";
        string contentType = "";

        if (request.AvatarImage != null && request.AvatarImage.Length > 0)
        {
            stream = request.AvatarImage.OpenReadStream();
            fileName = request.AvatarImage.FileName;
            contentType = request.AvatarImage.ContentType;
        }

        var result = await _service.RegisterAsync(request.Name, request.Email, request.Password, stream, fileName, contentType);

        return result.ToActionResult(id => Ok(id));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var result = await _service.LoginAsync(request.Email, request.Password);

        HttpContext.Response.Cookies.Append(_coockieName, result.IsFailure ? "" : result.Value!);

        return result.ToActionResult(token => Ok());
    }

    [Authorize]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id,[FromForm] UserRequest request)
    {
        Stream? stream = null;
        string fileName = "";
        string contentType = "";

        if (request.AvatarImage != null && request.AvatarImage.Length > 0)
        {
            stream = request.AvatarImage.OpenReadStream();
            fileName = request.AvatarImage.FileName;
            contentType = request.AvatarImage.ContentType;
        }

        var result = await _service.UpdateAsync(id, request.Name, request.Email, request.Password, stream, fileName, contentType);

        return result.ToActionResult(user => Ok());
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        
        return result.ToActionResult(user => NoContent());
    }
}
