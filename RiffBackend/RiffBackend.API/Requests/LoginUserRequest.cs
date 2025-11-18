namespace RiffBackend.API.Requests;

public sealed record LoginUserRequest(string Email, string Password);
