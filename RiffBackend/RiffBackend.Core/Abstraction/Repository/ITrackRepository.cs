using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Repository;

public interface ITrackRepository
{
    Task<Guid> AddTrackAsync(Track newTrack);
    Task<Guid> DeleteTrackAsync(Guid id);
    Task<Track?> GetTrackByIdAsync(Guid id);
    Task<List<Track>> GetTracksAsync();
    Task<List<Track>> GetTracksByTitleAsync(string title);
    Task<List<Track>> GetTracksByUserIdAsync(Guid id);
    Task<Guid> UpdateTrackAsync(Track newTrack);
    Task<Guid> AddLikeTrackAsync(Guid userId, Guid trackId);
    Task<Guid?> RemoveLikeTrackAsync(Guid userId, Guid trackId);
    Task<bool> IsLikedAsync(Guid userId, Guid trackId);
}
