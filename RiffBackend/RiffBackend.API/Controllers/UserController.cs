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
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);

        return result.ToActionResult(user => Ok(Envelope.Ok(new UserResponse(user.Id, user.Name, user.AvatarUrl))));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] UserRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.RegisterAsync(request.Name, request.Email, request.Password, request.AvatarImage!);

        return result.ToActionResult(id => Ok(Envelope.Ok(id)));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var validationResult = _loginValidator.Validate(request);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.LoginAsync(request.Email, request.Password);

        HttpContext.Response.Cookies.Append(_coockieName, result.IsFailure ? "" : result.Value!);

        return result.ToActionResult(token => Ok());
    }

    [Authorize]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id,[FromForm] UserRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.UpdateAsync(id, request.Name, request.Email, request.Password, request.AvatarImage!);

        return result.ToActionResult(user => Ok(Envelope.Ok()));
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        
        return result.ToActionResult(user => NoContent());
    }
}
