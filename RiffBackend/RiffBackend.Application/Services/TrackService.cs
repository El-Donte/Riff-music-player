using Microsoft.AspNetCore.Http;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;
using System.Security.Cryptography;

namespace RiffBackend.Application.Services;

public class TrackService(ITrackRepository repository, IFileStorageService fileStorage) : ITrackService
{
    private readonly ITrackRepository _repository = repository;
    private readonly IFileStorageService _fileStorage = fileStorage;

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

    public async Task<Result<Guid>> AddAsync(Guid id, string title, string author, Guid userId, IFormFile imageFile, IFormFile trackFile)
    {
        Track? clone = await _repository.GetTrackByIdAsync(id);

        if (clone != null)
        {
            return Errors.General.AlreadyExist();
        }

        Track track = Track.Create(id, title, author, userId);

        var error = await AddPath(track.TrackPath, trackFile, _fileStorage.UploadTrackFileAsync);
        if (error.Type != ErrorType.None)
        {
            return error;
        }

        error = await AddPath(track.ImagePath, imageFile, _fileStorage.UploadImageFileAsync);
        if (error.Type != ErrorType.None)
        {
            return error;
        }

        return await _repository.AddTrackAsync(track);
    }

    public async Task<Result<Guid>> UpdateAsync(Guid id, string title, string author, Guid userId, IFormFile imageFile, IFormFile trackFile)
    {
        Track? track = await _repository.GetTrackByIdAsync(id);

        if (track is null)
        {
            return Errors.TrackErrors.NotFound();
        }

        var result = await ValidateFileAsync(trackFile, track.TrackPath, _fileStorage.UploadTrackFileAsync);
        if (result.IsFailure)
        {
            return result.Error;
        }
        var trackPath = result.Value!;

        result = await ValidateFileAsync(imageFile, track.ImagePath, _fileStorage.UploadImageFileAsync);
        if (result.IsFailure)
        {
            return result.Error;
        }
        var imagePath = result.Value!;

        Track newTrack = Track.Create(id, title, author, userId);
        newTrack.TrackPath = trackPath;
        newTrack.ImagePath = imagePath;

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

        return await _repository.DeleteTrackAsync(id);
    }

    private async Task<Result<string>> ValidateFileAsync(IFormFile file, string oldPath,
                                                        Func<Stream, string, string, Task<Result<string>>> uploadFunc)
    {
        var etagResult = await _fileStorage.GetEtagAsync(oldPath);
        if (etagResult.IsFailure)
        {
            return etagResult;
        }

        using var stream = file.OpenReadStream();
        var newHash = GetFileMd5(stream);
        var oldHash = etagResult.Value!;

        if (newHash == oldHash)
        {
            return oldPath;
        }

        stream.Seek(0, SeekOrigin.Begin);

        return await uploadFunc(stream, file.FileName, file.ContentType);
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

    private async Task<Error> AddPath(string path, IFormFile file, Func<Stream, string , string, Task<Result<string>>> upload)
    {
        using var stream = file.OpenReadStream();
        var result = await upload(stream, file.FileName, file.ContentType);
        if (result.IsFailure)
        {
            return result.Error;
        }

        path = result.Value!;

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
