namespace RiffBackend.API.Requests;

public record UserRequest(string Name, string Email, string Password, IFormFile? AvatarImage);