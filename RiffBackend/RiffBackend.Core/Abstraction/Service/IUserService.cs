using RiffBackend.Core.Shared;
using RiffBackend.Core.Models;
using Microsoft.AspNetCore.Http;

namespace RiffBackend.Core.Abstraction.Service;

public interface IUserService
{
    Task<Result<Guid>> RegisterAsync(string name, string email, string password, IFormFile image);
    Task<Result<string>> LoginAsync(string email, string password);
    Task<Result<Guid>> UpdateAsync(Guid id,string name, string email, string password, IFormFile image);
    Task<Result<User>> GetUserAsync(string jwt);
    Task<Result<User>> GetByIdAsync(Guid id);
    Task<Result<Guid>> DeleteAsync(Guid id);
}
