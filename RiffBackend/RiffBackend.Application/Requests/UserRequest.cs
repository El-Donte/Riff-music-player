using Microsoft.AspNetCore.Http;

namespace RiffBackend.Application.Requests;

public record UserRequest(string Name, string Email, string Password, IFormFile? AvatarImage);