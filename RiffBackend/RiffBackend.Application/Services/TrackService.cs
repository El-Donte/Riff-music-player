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

    public async Task<Result<List<Track>>> GetAllAsync()
    {
        var tracks = await _repository.GetTracksAsync();

        if (tracks is null) 
        {
            return Errors.TrackErrors.NotFound();
        }

        foreach (var track in tracks)
        {
            var error = await _fileProcessor.EnrichWithUrlsAsync(track);
            if (error.Type != ErrorType.None){
               return error;
            }
        }

        return tracks;
    }

    public async Task<Result<List<Track>>> GetAllByTitleAsync(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return Errors.General.ValueIsRequired("Title");
        }

        var tracks = await _repository.GetTracksByTitleAsync(title);

        if (tracks is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        foreach (var track in tracks)
        {
            var error = await _fileProcessor.EnrichWithUrlsAsync(track);
            if (error.Type != ErrorType.None)
            {
                return error;
            }
        }

        return tracks;
    }

    public async Task<Result<List<Track>>> GetAllByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        var tracks = await _repository.GetTracksByUserIdAsync(userId);

        if (tracks is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        foreach (var track in tracks)
        {
            var error = await _fileProcessor.EnrichWithUrlsAsync(track);
            if (error.Type != ErrorType.None)
            {
                return error;
            }
        }

        return tracks;
    }

    public async Task<Result<Track>> GetById(Guid id)
    {
        if(id == Guid.Empty)
        {
            return Errors.TrackErrors.MissingId();
        }

        Track? track = await _repository.GetTrackByIdAsync(id);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        var error = await _fileProcessor.EnrichWithUrlsAsync(track);
        if (error.Type != ErrorType.None)
        {
            return error;
        }

        return track;
    }

    public async Task<Result<Guid>> LikeTrackAsync(Guid userId, Guid trackId)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        if(trackId == Guid.Empty)
        {
            return Errors.TrackErrors.MissingId();
        }

        Track? track = await _repository.GetTrackByIdAsync(trackId);

        if(track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        return await _repository.AddLikeTrackAsync(userId, trackId);
    }

    public async Task<Result<List<Track>>> GetLikedTracksAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        var tracks = await _repository.GetLikedAsync(userId);

        if (tracks is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        foreach (var track in tracks)
        {
            var error = await _fileProcessor.EnrichWithUrlsAsync(track);
            if (error.Type != ErrorType.None)
            {
                return error;
            }
        }

        return tracks;
    }

    public async Task<Result<Guid>> UnlikeTrackAsync(Guid userId, Guid trackId)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        if (trackId == Guid.Empty)
        {
            return Errors.TrackErrors.MissingId();
        }

        Track? track = await _repository.GetTrackByIdAsync(trackId);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        return await _repository.RemoveLikeTrackAsync(userId, trackId);
    }

    public async Task<Result<bool>> IsLikedAsync(Guid userId, Guid trackId)
    {
        if (userId == Guid.Empty)
        {
            return Errors.UserErrors.MissingId();
        }

        if (trackId == Guid.Empty)
        {
            return Errors.TrackErrors.MissingId();
        }

        Track? track = await _repository.GetTrackByIdAsync(trackId);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        return await _repository.IsLikedAsync(userId, trackId);
    }

    public async Task<Result<Guid>> AddAsync(Guid id, string title, string author, Guid userId, IFormFile imageFile, IFormFile trackFile)
    {
        Track? clone = await _repository.GetTrackByIdAsync(id);

        if (clone != null)
        {
            return Errors.General.AlreadyExist();
        }

        var trackPathResult = await _fileProcessor.UploadNewOrKeepOldAsync(trackFile, "", _storage.UploadTrackFileAsync);
        if (trackPathResult.IsFailure)
        {
            return trackPathResult.Error;
        }

        var imagePathResult = await _fileProcessor.UploadNewOrKeepOldAsync(imageFile, "", _storage.UploadImageFileAsync);
        if (imagePathResult.IsFailure)
        {
            return imagePathResult.Error;
        }

        return await _repository.AddTrackAsync(
            Track.Create(id, title, author, userId, trackPathResult.Value!, imagePathResult.Value!));
    }

    public async Task<Result<Guid>> UpdateAsync(Guid id, string title, string author, Guid userId, IFormFile imageFile, IFormFile trackFile)
    {
        Track? track = await _repository.GetTrackByIdAsync(id);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        var trackPathResult = await _fileProcessor.UploadNewOrKeepOldAsync(trackFile, track.TrackPath, _storage.UploadTrackFileAsync);
        if (trackPathResult.IsFailure)
        {
            return trackPathResult.Error;
        }

        var imagePathResult = await _fileProcessor.UploadNewOrKeepOldAsync(imageFile, track.ImagePath, _storage.UploadImageFileAsync);
        if (imagePathResult.IsFailure)
        {
            return imagePathResult.Error;
        }

        return await _repository.UpdateTrackAsync(
            Track.Create(id, title, author, userId, trackPathResult.Value!, imagePathResult.Value!));
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        Track? track = await _repository.GetTrackByIdAsync(id);

        if (track == null)
        {
            return Errors.TrackErrors.NotFound();
        }
        
        var trackResult = await _storage.DeleteFileAsync(track.TrackPath);
        if (trackResult.IsFailure)
        {
            return trackResult.Error;
        }

        var imageResult = await _storage.DeleteFileAsync(track.ImagePath);
        if (imageResult.IsFailure)
        {
            return imageResult.Error;
        }

        return await _repository.DeleteTrackAsync(id);
    }
}
