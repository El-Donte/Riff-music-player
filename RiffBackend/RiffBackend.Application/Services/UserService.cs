using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;

namespace RiffBackend.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository userRepository)
    {
        _repository = userRepository;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        User? user = await _repository.GetUserByIdAsync(id);

        if (user == null)
        {
            throw new Exception($"User with this id:{id} dosent exist");
        }

        return user;
    }

    public async Task<Guid> AddAsync(User user)
    {
        User? clone = await _repository.GetUserByIdAsync(user.Id);

        if (clone != null)
        {
            throw new Exception("User already exist");
        }

        clone = await _repository.GetByEmailAsync(user.Email);

        if (clone != null)
        {
            throw new Exception($"This email {user.Email} already used");
        }

        var result = await _repository.AddUserAsync(user);

        return result;
    }

    public async Task<Guid> UpdateAsync(User user)
    {
        User? clone = await _repository.GetUserByIdAsync(user.Id);

        if (clone == null)
        {
            throw new Exception($"User by id:{user.Id} doesnt exist");
        }

        clone = await _repository.GetByEmailAsync(user.Email);

        if (clone != null && clone.Id != user.Id)
        {
            throw new Exception($"This {user.Email} already used");
        }

        var result = await _repository.UpdateUserAsync(user);

        return result;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        User? user = await _repository.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new Exception($"User by id:{id} doesnt exist");
        }

        var result = await _repository.DeleteUserAsync(id);

        return result;
    }
}
