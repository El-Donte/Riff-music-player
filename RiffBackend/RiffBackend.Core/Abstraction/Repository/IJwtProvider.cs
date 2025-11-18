using RiffBackend.Core.Models;

namespace RiffBackend.Core.Abstraction.Repository;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
