using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Requests;
using RiffBackend.API.Responses;
using RiffBackend.Core.Abstraction;

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

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
    {
        var user = await _service.GetAsync(id);

        return Ok(new UserResponse(user.Id, user.Name, user.AvatarUrl));
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] UserRequest request)
    {
        //TO-DO так же валидации
        var user = Core.Models.User.Create(Guid.NewGuid(), request.Name, request.Email, request.Password, request.AvatarUrl);

        var id = await _service.AddAsync(user);

        return Ok(user.Id);
    }
}
