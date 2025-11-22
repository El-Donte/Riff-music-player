namespace RiffBackend.Application.Requests;

public record LikeTrackRequest(Guid UserId, Guid TrackId);