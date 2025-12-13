using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Models;
using RiffBackend.Infrastructure.Data;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.Repositories;

public class TrackRepository(ApplicationDbContext context, IMapper mapper) : ITrackRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Track?> GetTrackByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _context.Tracks
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<Track>(entity);
    }

    public async Task<Guid> AddLikeTrackAsync(Guid userId, Guid trackId, CancellationToken ct = default)
    {
        var entity = new LikedTracksEntity()
        {
            UserId = userId,
            TrackId = trackId,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.LikedTracks.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return trackId;
    }

    public async Task<List<Track>> GetLikedAsync(Guid userId, CancellationToken ct = default)
    {
        var entities = await _context.LikedTracks
            .AsNoTracking()
            .Include(t => t.Track)
            .Where(t => t.UserId == userId)
            .Select(t => t.Track)
            .ToListAsync(ct);

        return _mapper.Map<List<Track>>(entities);
    }

    public async Task<Guid?> RemoveLikeTrackAsync(Guid userId, Guid trackId, CancellationToken ct = default)
    {
        var entity = await _context.LikedTracks
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TrackId == trackId, ct);

        if (entity == null)
        {
            return null;
        }

        _context.LikedTracks.Remove(entity);
        await _context.SaveChangesAsync(ct);

        return trackId;
    }

    public async Task<bool> IsLikedAsync(Guid userId, Guid trackId, CancellationToken ct = default) 
        =>  await _context.LikedTracks.AnyAsync(l => l.TrackId == trackId && l.UserId == userId, ct);

    public async Task<List<Track>> GetTracksAsync(CancellationToken ct = default)
    {
        var entities = await _context.Tracks
            .AsNoTracking()
            .ToListAsync(ct);

        return _mapper.Map<List<Track>>(entities);
    }

    public async Task<List<Track>> GetTracksByTitleAsync(string title, CancellationToken ct = default)
    {
        List<TrackEntity> entities;

        if (string.IsNullOrEmpty(title))
        {
            entities = await _context.Tracks.AsNoTracking().ToListAsync(ct);
        }
        else
        {
            entities = await _context.Tracks
                .AsNoTracking()
                .Where(t => t.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync(ct);
        }
       
        return _mapper.Map<List<Track>>(entities);
    }

    public async Task<List<Track>> GetTracksByUserIdAsync(Guid id, CancellationToken ct = default)
    {
        var entities = await _context.Tracks
            .AsNoTracking()
            .Where(t => t.UserId == id)
            .ToListAsync(ct);

        return _mapper.Map<List<Track>>(entities);
    }

    public async Task<Guid> AddTrackAsync(Track newTrack, CancellationToken ct = default)
    {
        var entity = _mapper.Map<TrackEntity>(newTrack);

        await _context.Tracks.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return newTrack.Id;
    }

    public async Task<Guid> UpdateTrackAsync(Track newTrack, CancellationToken ct = default)
    {
        var entity = await _context.Tracks.FirstOrDefaultAsync(t => t.Id == newTrack.Id, ct);
        if (entity == null)
        {
            return Guid.Empty;
        }

        _mapper.Map(newTrack, entity);
        await _context.SaveChangesAsync(ct);

        return newTrack.Id;
    }

    public async Task<Guid> DeleteTrackAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _context.Tracks
            .Include(t => t.LikedByUsers)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        if (entity is null)
        {
            return Guid.Empty;
        }

        _context.LikedTracks.RemoveRange(entity.LikedByUsers);
        _context.Tracks.Remove(entity);

        await _context.SaveChangesAsync(ct);

        return entity.Id;
    }
}
