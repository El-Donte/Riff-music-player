using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Extensions;
using RiffBackend.API.Responses;
using RiffBackend.Application.Requests;
using RiffBackend.Core.Abstraction.Service;

namespace RiffBackend.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService service, 
                            IConfiguration configuration, 
                            IValidator<UserRequest> validator, 
                            IValidator<LoginUserRequest> loginValidator) : Controller
{
    private readonly IUserService _service = service;
    private readonly IValidator<UserRequest> _validator = validator;
    private readonly IValidator<LoginUserRequest> _loginValidator = loginValidator;

    private readonly string _coockieName = configuration["Authentication:CookieName"]
               ?? throw new InvalidOperationException("CookieName is missing!");

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);

        return result.ToActionResult(user => 
            Ok(Envelope.Ok(new UserResponse(user.Id, user.Name, user.AvatarPath))));
    }

    [Authorize]
    [HttpGet("")]
    public async Task<IActionResult> GetUser(CancellationToken ct)
    {
        var jwt = Request.Cookies[_coockieName];
        
        var result = await _service.GetUserFromJwtAsync(jwt, ct);

        return result.ToActionResult(user => 
            Ok(Envelope.Ok(new UserResponse(user.Id, user.Name, user.AvatarPath))));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] UserRequest request, CancellationToken ct)
    {
        var validationResult = _validator.Validate(request);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.RegisterAsync(Guid.NewGuid(),request.Name, request.Email, 
            request.Password, request.AvatarImage!, ct);

        return result.ToActionResult(id => Ok(Envelope.Ok(id)));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken ct)
    {
        var validationResult = await _loginValidator.ValidateAsync(request, ct);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.LoginAsync(request.Email, request.Password, ct);

        HttpContext.Response.Cookies.Append(_coockieName, result.IsFailure ? "" : result.Value!);

        return result.ToActionResult(t => Ok(Envelope.Ok(t)));
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(_coockieName);

        return Ok(Envelope.Ok("Вы вышли из аккаунта"));
    }

    [Authorize]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id,[FromForm] UserRequest request, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.UpdateAsync(id, request.Name, request.Email, 
            request.Password, request.AvatarImage!, ct);

        return result.ToActionResult(id => Ok(Envelope.Ok(id)));
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken ct)
    {
        var result = await _service.DeleteAsync(id, ct);
        
        return result.ToActionResult(user => NoContent());
    }
}
