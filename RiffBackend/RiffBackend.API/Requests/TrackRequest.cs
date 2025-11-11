namespace RiffBackend.API.Requests;

public record TrackRequest(string Title, string Author, string TrackPath, string ImagePath, Guid UserId, IFormFile file);
