using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Repository
{
    public interface IUserRepository
    {
        Task<Guid> AddUserAsync(User newUser);
        Task<Guid> DeleteUserAsync(Guid id);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<Guid> UpdateUserAsync(User newUser);
    }
}