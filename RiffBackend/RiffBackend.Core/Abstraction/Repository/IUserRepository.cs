using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Repository;

public interface IUserRepository
{
    Task<EmailVerificationToken> AddUserAsync(User newUser, CancellationToken ct = default);
    Task<Guid> DeleteUserAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Guid> UpdateUserAsync(User newUser, CancellationToken ct = default);
    Task<EmailVerificationToken> GetEmailVerificationTokenAsync(Guid id, CancellationToken ct = default);
    Task<Guid> DeleteEmailVerificationTokenAsync(Guid id, CancellationToken ct = default);
}
