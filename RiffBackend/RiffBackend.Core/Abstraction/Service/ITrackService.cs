using Microsoft.AspNetCore.Http;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;

namespace RiffBackend.Core.Abstraction.Service;

public interface ITrackService
{
    Task<Result<Guid>> AddAsync(Guid id, string title, string author, Guid userId, IFormFile imageFile, IFormFile trackFile);
    Task<Result<Guid>> DeleteAsync(Guid id);
    Task<Result<List<Track>>> GetAllAsync();
    Task<Result<Track>> GetById(Guid id);
    Task<Result<Guid>> UpdateAsync(Guid id, string title, string author, Guid userId, IFormFile imageFile, IFormFile trackFile);
    Task<Result<Guid>> LikeTrackAsync(Guid userId, Guid trackId);
    Task<Result<Guid>> UnlikeTrackAsync(Guid userId, Guid trackId);
    Task<Result<bool>> IsLikedAsync(Guid userId, Guid trackId);
}
