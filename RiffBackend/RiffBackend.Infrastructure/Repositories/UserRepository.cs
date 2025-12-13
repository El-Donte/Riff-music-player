using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Models;
using RiffBackend.Infrastructure.Data;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context, IMapper mapper) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        UserEntity? entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<User>(entity);
    }

    public async Task<Guid> AddUserAsync(User newUser, CancellationToken ct = default)
    {
        var entity = _mapper.Map<UserEntity>(newUser);

        await _context.Users.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task<Guid> UpdateUserAsync(User newUser, CancellationToken ct = default)
    {
        UserEntity? entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == newUser.Id, ct);
        if (entity == null)
        {
            return Guid.Empty;
        }

        _mapper.Map(newUser, entity);
        await _context.SaveChangesAsync(ct);

        return newUser.Id;
    }

    public async Task<Guid> DeleteUserAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        if (entity == null)
        {
            return Guid.Empty;
        }

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        UserEntity? entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);

        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<User>(entity);
    }
}

