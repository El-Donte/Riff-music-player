using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Requests;
using RiffBackend.API.Responses;
using RiffBackend.Core.Abstraction.Service;
using System.Diagnostics.Metrics;

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
        var user = await _service.GetAsync(id);

        return Ok(new UserResponse(user.Id, user.Name, user.AvatarUrl));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
    {
        //TO-DO так же валидации
        var user = Core.Models.User.Create(Guid.NewGuid(), request.Name, request.Email, request.Password, request.AvatarUrl);

        var id = await _service.AddAsync(user);

        return Created();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id,[FromBody] UserRequest request)
    {
        var user = Core.Models.User.Create(id, request.Name, request.Email, request.Password, request.AvatarUrl);
        await _service.UpdateAsync(user);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _service.DeleteAsync(id);
        
        return NoContent();
    }
}
