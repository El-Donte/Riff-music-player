using RiffBackend.Core.Shared;
using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Service
{
    public interface IUserService
    {
        Task<Result<Guid>> RegisterAsync(string name, string email, string password, Stream stream, string fileName, string contentType);
        Task<Result<string>> LoginAsync(string email, string password);
        Task<Result<Guid>> UpdateAsync(Guid id,string name, string email, string password, Stream stream, string fileName, string contentType);
        Task<Result<User>> GetByIdAsync(Guid id);
        Task<Result<Guid>> DeleteAsync(Guid id);
    }
}