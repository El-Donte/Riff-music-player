namespace RiffBackend.API.Responses;

public record TrackResponse(Guid id, string Title, string Author, string TrackPath, string ImagePath, DateTime? CreatedAt);
