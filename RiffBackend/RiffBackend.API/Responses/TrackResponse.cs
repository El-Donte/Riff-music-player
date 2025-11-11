namespace RiffBackend.API.Responses;

public record TrackResponse(string Title, string Author, string TrackPath, string ImagePath, DateTime? CreatedAt);
