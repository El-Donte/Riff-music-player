using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;
using System.Security.Cryptography;

namespace RiffBackend.Application.Services;

public class TrackService : ITrackService
{
    private readonly ITrackRepository _repository;
    private readonly IFileStorageService _fileStorage;

    public TrackService(ITrackRepository repository, IFileStorageService fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<Result<List<Track>>> GetAllAsync()
    {
        var tracks = await _repository.GetTracksAsync();

        if (tracks is null) 
        {
            return Errors.TrackErrors.NotFound();
        }

        foreach (var track in tracks)
        {
            var error = await AddUrls(track);
            if (error.Type != ErrorType.None){
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

        var error = await AddUrls(track);
        if (error.Type != ErrorType.None)
        {
            return error;
        }

        return track;
    }

    public async Task<Result<Guid>> AddAsync(Track track, Stream trackStream, string trackFileName, string trackContentType,
                                                          Stream imageStream, string imageFileName, string imageContentType)
    {
        Track? clone = await _repository.GetTrackByIdAsync(track.Id);

        if (clone != null)
        {
            return Errors.General.AlreadyExist();
        }

        var error = await AddPaths(track, trackStream, trackFileName, trackContentType, imageStream, imageFileName, imageContentType);
        if(error.Type != ErrorType.None)
        {
            return error;
        }

        return await _repository.AddTrackAsync(track);
    }

    public async Task<Result<Guid>> UpdateAsync(Track newTrack, Stream trackStream, string trackFileName, string trackContentType,
                                                                Stream imageStream, string imageFileName, string imageContentType)
    {
        Track? track = await _repository.GetTrackByIdAsync(newTrack.Id);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        var trackUploadResult = await ValidateFileAsync(trackStream, trackFileName, trackContentType, track.TrackPath, _fileStorage.UploadTrackFileAsync);
        if (trackUploadResult.IsFailure)
        {
            return trackUploadResult.Error;
        }
        track.TrackPath = trackUploadResult.Value!;

        var imageUploadResult = await ValidateFileAsync(imageStream, imageFileName, imageContentType, track.ImagePath, _fileStorage.UploadImageFileAsync);
        if (imageUploadResult.IsFailure)
        {
            return imageUploadResult.Error;
        }
        track.ImagePath = imageUploadResult.Value!;

        return await _repository.UpdateTrackAsync(newTrack);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        Track? track = await _repository.GetTrackByIdAsync(id);

        if (track == null)
        {
            return Errors.TrackErrors.NotFound();
        }
        
        var trackResult = await _fileStorage.DeleteFileAsync(track.TrackPath);
        if (trackResult.IsFailure)
        {
            return trackResult.Error;
        }

        var imageResult = await _fileStorage.DeleteFileAsync(track.ImagePath);
        if (imageResult.IsFailure)
        {
            return imageResult.Error;
        }

        return await _repository.DeleteTrackAsync(id); ;
    }

    private async Task<Result<string>> ValidateFileAsync(Stream stream, string fileName, string contentType, string oldPath,
                                                        Func<Stream, string, string, Task<Result<string>>> uploadFunc)
    {
        if (stream == null || stream.Length == 0)
        {
            return oldPath;
        }

        var etagResult = await _fileStorage.GetEtagAsync(oldPath);
        if (etagResult.IsFailure)
        {
            return etagResult;
        }

        var newHash = GetFileMd5(stream);
        var oldHash = etagResult.Value!;

        if (newHash == oldHash)
        {
            return oldPath;
        }

        stream.Seek(0, SeekOrigin.Begin);
        var uploadResult = await uploadFunc(stream, fileName, contentType);
        return uploadResult;
    }

    private async Task<Error> AddUrls(Track track)
    {
        var trackUrlResult = await _fileStorage.GetURLAsync(track.TrackPath);
        if (trackUrlResult.IsFailure) return trackUrlResult.Error;

        var imageUrlResult = await _fileStorage.GetURLAsync(track.ImagePath);
        if (imageUrlResult.IsFailure) return imageUrlResult.Error;

        track.TrackPath = trackUrlResult.Value!;
        track.ImagePath = imageUrlResult.Value!;

        return Error.None();
    }

    private async Task<Error> AddPaths(Track track, Stream trackStream, string trackFileName, string trackContentType,
                                                   Stream imageStream, string imageFileName, string imageContentType)
    {
        var imageUploadResult = await _fileStorage.UploadImageFileAsync(imageStream, imageFileName, imageContentType);
        if (imageUploadResult.IsFailure)
        {
            return imageUploadResult.Error;
        }

        var trackUploadResult = await _fileStorage.UploadTrackFileAsync(trackStream, trackFileName, trackContentType);
        if (trackUploadResult.IsFailure)
        {
            return trackUploadResult.Error;
        }

        track.TrackPath = trackUploadResult.Value!;
        track.ImagePath = imageUploadResult.Value!;

        return Error.None();
    }

    private string GetFileMd5(Stream stream)
    {
        using (var md5 = MD5.Create())
        using (stream)
        {
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
