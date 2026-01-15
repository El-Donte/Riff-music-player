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
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<User>(entity);
    }

    public async Task<EmailVerificationToken> AddUserAsync(User newUser, CancellationToken ct = default)
    {
        var entity = _mapper.Map<UserEntity>(newUser);
        
        var verificationToken = new EmailVerificationTokenEntity()
        {
            Id = Guid.NewGuid(),
            UserId = newUser.Id,
            CreatedAtUtc =  DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(1),
            User = entity
        };
        
        await _context.EmailVerificationTokens.AddAsync(verificationToken, ct);
        await _context.Users.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return new EmailVerificationToken()
        {
            Id = verificationToken.Id,
            UserId = verificationToken.User!.Id,
            CreatedAtUtc = verificationToken.CreatedAtUtc,
            ExpiresAtUtc = verificationToken.ExpiresAtUtc,
            User = _mapper.Map<User>(verificationToken.User),
        };
    }

    public async Task<EmailVerificationToken> GetEmailVerificationTokenAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _context.EmailVerificationTokens
            .AsNoTracking()
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (entity == null)
        {
            return null;
        }
        
        return new EmailVerificationToken()
        {
            Id = entity.Id,
            UserId = entity.User!.Id,
            CreatedAtUtc = entity.CreatedAtUtc,
            ExpiresAtUtc = entity.ExpiresAtUtc,
            User = _mapper.Map<User>(entity.User),
        };
    }

    public async Task<Guid> DeleteEmailVerificationTokenAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _context.EmailVerificationTokens
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (entity == null)
        {
            return Guid.Empty;
        }
        
        entity.User!.EmailVerified = true;
        _context.EmailVerificationTokens.Remove(entity);
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

