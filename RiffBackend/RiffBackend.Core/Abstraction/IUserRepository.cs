using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction
{
    public interface IUserRepository
    {
        Task<Guid> AddUserAsync(User newUser);
        Task<Guid> DeleteUserAsync(Guid id);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<Guid> UpdateUserAsync(Guid id, User newUser);
    }
}