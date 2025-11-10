using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction
{
    public interface IUserService
    {
        Task<Guid> AddAsync(User user);
        Task<Guid> DeleteAsync(Guid id);
        Task<User> GetAsync(Guid id);
        Task<Guid> UpdateAsync(Guid id, User user);
    }
}