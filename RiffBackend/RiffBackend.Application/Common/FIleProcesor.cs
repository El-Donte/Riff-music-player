using Microsoft.AspNetCore.Http;
using RiffBackend.Application.Extensions;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Common;

public class FileProcessor(IFileStorageService storage) : IFileProcessor
{
    private readonly IFileStorageService _storage = storage;

    public async Task<Result<string>> UploadNewOrKeepOldAsync(
        IFormFile? file,
        string oldPath,
        Func<Stream, string, string, Task<Result<string>>> uploadFunc)
    {
        if (file == null || file.Length == 0)
        {
            return oldPath;
        }

        using var stream = file.OpenReadStream();
        if (oldPath != "")
        {
            var etagResult = await _storage.GetEtagAsync(oldPath);
            if (etagResult.IsFailure)
            {
                return etagResult;
            }

            var newHash = stream.ComputeMD5();

            if (newHash == etagResult.Value)
            {
                return oldPath;
            }
        }

        stream.Seek(0, SeekOrigin.Begin);
        return await uploadFunc(stream, file.FileName, file.ContentType);
    }

    public async Task<Error> EnrichWithUrlsAsync(Track track)
    {
        var trackUrlResult = await _storage.GetURLAsync(track.TrackPath);
        if (trackUrlResult.IsFailure) return trackUrlResult.Error;

        var imageUrlResult = await _storage.GetURLAsync(track.ImagePath);
        if (imageUrlResult.IsFailure) return imageUrlResult.Error;

        track.TrackPath = trackUrlResult.Value!;
        track.ImagePath = imageUrlResult.Value!;

        return Error.None();
    }
}
