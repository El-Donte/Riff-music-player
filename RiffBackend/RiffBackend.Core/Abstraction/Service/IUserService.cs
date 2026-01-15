using RiffBackend.Core.Shared;
using RiffBackend.Core.Models;
using Microsoft.AspNetCore.Http;

namespace RiffBackend.Core.Abstraction.Service;

public interface IUserService
{
    Task<Result<Guid>> RegisterAsync(Guid id, string name, string email, 
        string password, IFormFile image, CancellationToken ct);
    Task<Result<string>> LoginAsync(string email, string password, CancellationToken ct);
    Task<Result<Guid>> UpdateAsync(Guid id,string name, string email, 
        string password, IFormFile image, CancellationToken ct);
    Task<Result<User>> GetUserFromJwtAsync(string jwt, CancellationToken ct);
    Task<Result<User>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken ct);
    Task<Result<bool>> VerifyEmailAsync(Guid tokenId, CancellationToken ct = default);
}
