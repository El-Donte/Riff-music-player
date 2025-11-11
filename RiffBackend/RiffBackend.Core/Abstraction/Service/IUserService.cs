using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Service
{
    public interface IUserService
    {
        Task<Guid> AddAsync(User user);
        Task<Guid> DeleteAsync(Guid id);
        Task<User> GetAsync(Guid id);
        Task<Guid> UpdateAsync(User user);
    }
}