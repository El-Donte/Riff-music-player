using Microsoft.AspNetCore.Http;
using RiffBackend.Application.Common;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Services;

public class TrackService(ITrackRepository repository, IFileProcessor fileProcessor, IFileStorageService storageService) : ITrackService
{
    private readonly ITrackRepository _repository = repository;
    private readonly IFileProcessor _fileProcessor = fileProcessor;
    private readonly IFileStorageService _storage = storageService;

    public async Task<Result<List<Track>>> GetAllAsync(CancellationToken ct = default)
    {
        var tracks = await _repository.GetTracksAsync(ct);

        if (tracks is null) 
        {
            return Errors.TrackErrors.NotFound();
        }

        foreach (var track in tracks)
        {
            var error = await _fileProcessor.EnrichWithUrlsAsync(track, ct);
            if (error.Type != ErrorType.None){
               return error;
            }
        }

        return tracks;
    }

    public async Task<Result<List<Track>>> GetAllByTitleAsync(string title, CancellationToken ct = default)
    {
        if (title is null)
        {
            return Errors.General.ValueIsRequired("Title");
        }

        var tracks = await _repository.GetTracksByTitleAsync(title, ct);

        if (tracks is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        foreach (var track in tracks)
        {
            var error = await _fileProcessor.EnrichWithUrlsAsync(track, ct);
            if (error.Type != ErrorType.None)
            {
                return error;
            }
        }

        return tracks;
    }

    public async Task<Result<List<Track>>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        var tracks = await _repository.GetTracksByUserIdAsync(userId, ct);

        if (tracks is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        foreach (var track in tracks)
        {
            var error = await _fileProcessor.EnrichWithUrlsAsync(track, ct);
            if (error.Type != ErrorType.None)
            {
                return error;
            }
        }

        return tracks;
    }

    public async Task<Result<Track>> GetById(Guid id, CancellationToken ct = default)
    {
        if(id == Guid.Empty)
        {
            return Errors.TrackErrors.MissingId();
        }

        Track? track = await _repository.GetTrackByIdAsync(id, ct);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        var error = await _fileProcessor.EnrichWithUrlsAsync(track, ct);
        if (error.Type != ErrorType.None)
        {
            return error;
        }

        return track;
    }

    public async Task<Result<Guid>> LikeTrackAsync(Guid userId, Guid trackId, CancellationToken ct = default)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        if(trackId == Guid.Empty)
        {
            return Errors.TrackErrors.MissingId();
        }

        Track? track = await _repository.GetTrackByIdAsync(trackId, ct);

        if(track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        return await _repository.AddLikeTrackAsync(userId, trackId, ct);
    }

    public async Task<Result<List<Track>>> GetLikedTracksAsync(Guid userId, CancellationToken ct = default)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        var tracks = await _repository.GetLikedAsync(userId, ct);

        if (tracks is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        foreach (var track in tracks)
        {
            var error = await _fileProcessor.EnrichWithUrlsAsync(track, ct);
            if (error.Type != ErrorType.None)
            {
                return error;
            }
        }

        return tracks;
    }

    public async Task<Result<Guid>> UnlikeTrackAsync(Guid userId, Guid trackId, CancellationToken ct = default)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        if (trackId == Guid.Empty)
        {
            return Errors.TrackErrors.MissingId();
        }

        Track? track = await _repository.GetTrackByIdAsync(trackId, ct);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        return await _repository.RemoveLikeTrackAsync(userId, trackId, ct);
    }

    public async Task<Result<bool>> IsLikedAsync(Guid userId, Guid trackId, CancellationToken ct = default)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        if (trackId == Guid.Empty)
        {
            return Errors.TrackErrors.MissingId();
        }

        Track? track = await _repository.GetTrackByIdAsync(trackId, ct);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        return await _repository.IsLikedAsync(userId, trackId, ct);
    }

    public async Task<Result<Guid>> AddAsync(Guid id, string title, string author, 
        Guid userId, IFormFile imageFile, IFormFile trackFile, CancellationToken ct = default)
    {
        if (trackFile == null)
        {
            return Errors.FileErrors.MissingFile("Track");
        }

        if (imageFile == null)
        {
            return Errors.FileErrors.MissingFile("Image");
        }

        Track? clone = await _repository.GetTrackByIdAsync(id, ct);

        if (clone != null)
        {
            return Errors.General.AlreadyExist();
        }

        var trackPathResult = await _fileProcessor.UploadNewOrKeepOldAsync(trackFile,
            "", ct, _storage.UploadTrackFileAsync);
        if (trackPathResult.IsFailure)
        {
            return trackPathResult.Error;
        }

        var imagePathResult = await _fileProcessor.UploadNewOrKeepOldAsync(imageFile, 
            "", ct, _storage.UploadImageFileAsync);
        if (imagePathResult.IsFailure)
        {
            return imagePathResult.Error;
        }

        return await _repository.AddTrackAsync(Track.Create(id, title, author, userId, 
            trackPathResult.Value!, imagePathResult.Value!), ct);
    }

    public async Task<Result<Guid>> UpdateAsync(Guid id, string title, string author, 
        Guid userId, IFormFile imageFile, IFormFile trackFile, CancellationToken ct = default)
    {
        Track? track = await _repository.GetTrackByIdAsync(id, ct);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        var trackPathResult = await _fileProcessor.UploadNewOrKeepOldAsync(trackFile, 
            track.TrackPath, ct, _storage.UploadTrackFileAsync);
        if (trackPathResult.IsFailure)
        {
            return trackPathResult.Error;
        }

        var imagePathResult = await _fileProcessor.UploadNewOrKeepOldAsync(imageFile, 
            track.ImagePath, ct, _storage.UploadImageFileAsync);
        if (imagePathResult.IsFailure)
        {
            return imagePathResult.Error;
        }

        return await _repository.UpdateTrackAsync(Track.Create(id, title, author, 
            userId, trackPathResult.Value!, imagePathResult.Value!), ct);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        Track? track = await _repository.GetTrackByIdAsync(id, ct);

        if (track == null)
        {
            return Errors.TrackErrors.NotFound();
        }
        
        var trackResult = await _storage.DeleteFileAsync(track.TrackPath, ct);
        if (trackResult.IsFailure)
        {
            return trackResult.Error;
        }

        var imageResult = await _storage.DeleteFileAsync(track.ImagePath, ct);
        if (imageResult.IsFailure)
        {
            return imageResult.Error;
        }

        return await _repository.DeleteTrackAsync(id, ct);
    }
}
