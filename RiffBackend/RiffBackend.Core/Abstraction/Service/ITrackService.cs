using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Service
{
    public interface ITrackService
    {
        Task<Guid> AddAsync(Track track);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<Track>> GetAllAsync();
        Task<Track> GetById(Guid id);
        Task<Guid> UpdateAsync(Track newTrack);
    }
}