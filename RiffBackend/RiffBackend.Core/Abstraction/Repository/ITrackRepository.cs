using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Repository;

public interface ITrackRepository
{
    Task<Guid> AddTrackAsync(Track newTrack, CancellationToken ct);
    Task<Guid> DeleteTrackAsync(Guid id, CancellationToken ct);
    Task<Track?> GetTrackByIdAsync(Guid id, CancellationToken ct);
    Task<List<Track>> GetTracksAsync(CancellationToken ct);
    Task<List<Track>> GetTracksByTitleAsync(string title, CancellationToken ct);
    Task<List<Track>> GetTracksByUserIdAsync(Guid id, CancellationToken ct);
    Task<Guid> UpdateTrackAsync(Track newTrack, CancellationToken ct);
    Task<Guid> AddLikeTrackAsync(Guid userId, Guid trackId, CancellationToken ct);
    Task<List<Track>> GetLikedAsync(Guid userId, CancellationToken ct);
    Task<Guid?> RemoveLikeTrackAsync(Guid userId, Guid trackId, CancellationToken ct);
    Task<bool> IsLikedAsync(Guid userId, Guid trackId, CancellationToken ct);
}
