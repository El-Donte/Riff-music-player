using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;

namespace RiffBackend.Core.Abstraction.Service
{
    public interface ITrackService
    {
        Task<Result<Guid>> AddAsync(Track track, Stream trackStream, string trackFileName, string trackContentType, 
                                                 Stream imageStream, string imageFileName, string imageContentType);
        Task<Result<Guid>> DeleteAsync(Guid id);
        Task<Result<List<Track>>> GetAllAsync();
        Task<Result<Track>> GetById(Guid id);
        Task<Result<Guid>> UpdateAsync(Track track, Stream trackStream, string trackFileName, string trackContentType,
                                                 Stream imageStream, string imageFileName, string imageContentType);
    }
}