namespace RiffBackend.API.Requests;

public record TrackRequest(string Title, string Author, Guid UserId, IFormFile TrackFile, IFormFile ImageFile);
