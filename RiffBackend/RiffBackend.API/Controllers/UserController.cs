using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Extensions;
using RiffBackend.API.Requests;
using RiffBackend.API.Responses;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Shared;

namespace RiffBackend.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;    
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        return result.ToActionResult(user => Ok(new UserResponse(user.Id, user.Name, user.AvatarUrl)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromForm] UserRequest request)
    {
        var image = request.AvatarImage;
        if (image is null) {
            return BadRequest(Errors.FileErrors.MissingFile());
        }

        var user = Core.Models.User.Create(Guid.NewGuid(), request.Name, request.Email, request.Password);
        using var stream = image.OpenReadStream();

        var result = await _service.AddAsync(user, stream, image.FileName, image.ContentType);

        return result.ToActionResult(user => Ok());
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id,[FromForm] UserRequest request)
    {
        var image = request.AvatarImage;
        if (image is null)
        {
            return BadRequest(Errors.FileErrors.MissingFile());
        }

        var user = Core.Models.User.Create(id, request.Name, request.Email, request.Password);
        using var stream = image.OpenReadStream();

        var result = await _service.UpdateAsync(user, stream, image.FileName, image.ContentType);
        return result.ToActionResult(user => Ok());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        
        return result.ToActionResult(user => NoContent());
    }
}
