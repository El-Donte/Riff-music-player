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

    public async Task<Track?> GetTrackByIdAsync(Guid id)
    {
        var entity = await _context.Tracks
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<Track>(entity);
    }

    public async Task<Guid> AddLikeTrackAsync(Guid userId, Guid trackId)
    {
        var entity = new LikedTracksEntity()
        {
            UserId = userId,
            TrackId = trackId,
            CreatedAt = DateTime.UtcNow,
        };

        _context.LikedTracks.Add(entity);
        await _context.SaveChangesAsync();

        return trackId;
    }

    public async Task<List<Track>> GetLikedAsync(Guid userId)
    {
        var entities = await _context.LikedTracks
            .AsNoTracking()
            .Include(t => t.Track)
            .Where(t => t.UserId == userId)
            .Select(t => t.Track)
            .ToListAsync();

        return _mapper.Map<List<Track>>(entities);
    }

    public async Task<Guid?> RemoveLikeTrackAsync(Guid userId, Guid trackId)
    {
        var entity = await _context.LikedTracks
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TrackId == trackId);

        if (entity == null)
        {
            return null;
        }

        _context.LikedTracks.Remove(entity);
        await _context.SaveChangesAsync();

        return trackId;
    }

    public async Task<bool> IsLikedAsync(Guid userId, Guid trackId) 
        =>  await _context.LikedTracks.AnyAsync(l => l.TrackId == trackId && l.UserId == userId);

    public async Task<List<Track>> GetTracksAsync()
    {
        var entities = await _context.Tracks
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<Track>>(entities);
    }

    public async Task<List<Track>> GetTracksByTitleAsync(string title)
    {
        List<TrackEntity> entities;

        if (string.IsNullOrEmpty(title))
        {
            entities = await _context.Tracks.AsNoTracking().ToListAsync();
        }
        else
        {
            entities = await _context.Tracks
                .AsNoTracking()
                .Where(t => t.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();
        }
       
        return _mapper.Map<List<Track>>(entities);
    }

    public async Task<List<Track>> GetTracksByUserIdAsync(Guid id)
    {
        var entities = await _context.Tracks
            .AsNoTracking()
            .Where(t => t.UserId == id)
            .ToListAsync();

        return _mapper.Map<List<Track>>(entities);
    }

    public async Task<Guid> AddTrackAsync(Track newTrack)
    {
        var entity = _mapper.Map<TrackEntity>(newTrack);

        _context.Tracks.Add(entity);
        await _context.SaveChangesAsync();

        return newTrack.Id;
    }

    public async Task<Guid> UpdateTrackAsync(Track newTrack)
    {
        var entity = await _context.Tracks.FirstOrDefaultAsync(t => t.Id == newTrack.Id);
        if (entity == null)
        {
            return Guid.Empty;
        }

        _mapper.Map(newTrack, entity);
        await _context.SaveChangesAsync();

        return newTrack.Id;
    }

    public async Task<Guid> DeleteTrackAsync(Guid id)
    {
        var entity = await _context.Tracks
            .Include(t => t.LikedByUsers)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (entity is null)
        {
            return Guid.Empty;
        }

        _context.LikedTracks.RemoveRange(entity.LikedByUsers);
        _context.Tracks.Remove(entity);

        await _context.SaveChangesAsync();

        return entity.Id;
    }
}
