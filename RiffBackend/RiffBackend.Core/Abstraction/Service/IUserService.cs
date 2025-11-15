using RiffBackend.Core.Shared;
using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Service
{
    public interface IUserService
    {
        Task<Result<Guid>> AddAsync(User user, Stream stream, string fileName, string contentType);
        Task<Result<Guid>> DeleteAsync(Guid id);
        Task<Result<User>> GetByIdAsync(Guid id);
        Task<Result<Guid>> UpdateAsync(User user, Stream stream, string fileName, string contentType)
    }
}