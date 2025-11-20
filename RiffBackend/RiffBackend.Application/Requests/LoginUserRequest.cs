namespace RiffBackend.Application.Requests;

public sealed record LoginUserRequest(string Email, string Password);
