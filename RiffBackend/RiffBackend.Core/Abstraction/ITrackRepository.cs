using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction
{
    public interface ITrackRepository
    {
        Task<Guid> AddTrackAsync(Track newTrack);
        Task<Guid> DeleteTrackAsync(Guid id);
        Task<Track?> GetTrackByIdAsync(Guid id);
        Task<List<Track>> GetTracksAsync();
        Task<Guid> UpdateTrackAsync(Track newTrack);
    }
}