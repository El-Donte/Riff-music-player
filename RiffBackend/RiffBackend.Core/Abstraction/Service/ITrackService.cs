using Microsoft.AspNetCore.Http;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;

namespace RiffBackend.Core.Abstraction.Service;

public interface ITrackService
{
    Task<Result<Guid>> AddAsync(Guid id, string title, string author, 
        Guid userId, IFormFile imageFile, IFormFile trackFile, CancellationToken ct);
    Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken ct);
    Task<Result<List<Track>>> GetAllAsync(CancellationToken ct);
    Task<Result<List<Track>>> GetAllByTitleAsync(string title, CancellationToken ct);
    Task<Result<List<Track>>> GetAllByUserIdAsync(Guid userId, CancellationToken ct);
    Task<Result<Track>> GetById(Guid id, CancellationToken ct);
    Task<Result<Guid>> UpdateAsync(Guid id, string title, string author, 
        Guid userId, IFormFile imageFile, IFormFile trackFile, CancellationToken ct);
    Task<Result<Guid>> LikeTrackAsync(Guid userId, Guid trackId, CancellationToken ct);
    Task<Result<List<Track>>> GetLikedTracksAsync(Guid userId, CancellationToken ct);
    Task<Result<Guid>> UnlikeTrackAsync(Guid userId, Guid trackId, CancellationToken ct);
    Task<Result<bool>> IsLikedAsync(Guid userId, Guid trackId, CancellationToken ct);
}
