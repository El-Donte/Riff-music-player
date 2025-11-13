using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Models;
using RiffBackend.Infrastructure.Data;
using RiffBackend.Infrastructure.Entities;

namespace RiffBackend.Infrastructure.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TrackRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

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

    public async Task<List<Track>> GetTracksAsync()
    {
        var entities = await _context.Tracks.ToListAsync();
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
