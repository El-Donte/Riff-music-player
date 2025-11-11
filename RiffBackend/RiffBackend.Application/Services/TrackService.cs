using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;

namespace RiffBackend.Application.Services;

public class TrackService : ITrackService
{
    private readonly ITrackRepository _repository;

    public TrackService(ITrackRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Track>> GetAllAsync()
    {
        return await _repository.GetTracksAsync();
    }

    public async Task<Track> GetById(Guid id)
    {
        Track? track = await _repository.GetTrackByIdAsync(id);

        if (track == null)
        {
            throw new ArgumentNullException(nameof(track));
        }

        return track;
    }

    public async Task<Guid> AddAsync(Track track)
    {
        Track? clone = await _repository.GetTrackByIdAsync(track.Id);

        if (clone != null)
        {
            throw new Exception($"Track with id: {track.Id} already exist");
        }

        var result = await _repository.AddTrackAsync(track);

        return result;
    }

    public async Task<Guid> UpdateAsync(Track newTrack)
    {
        Track? track = await _repository.GetTrackByIdAsync(newTrack.Id);

        if (track == null)
        {
            throw new Exception($"Track with id: {newTrack.Id} doesnt exist");
        }

        var result = await _repository.UpdateTrackAsync(newTrack);

        return result;
    }

    public async Task<Guid> DeleteAsync(Guid id)
    {
        Track? track = await _repository.GetTrackByIdAsync(id);

        if (track == null)
        {
            throw new Exception($"Track with id: {id} doesnt exist");
        }

        var result = await _repository.DeleteTrackAsync(id);

        return result;
    }
}
