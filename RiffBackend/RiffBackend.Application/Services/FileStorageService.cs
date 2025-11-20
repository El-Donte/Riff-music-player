using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Services;

public class FileStorageService(IFileStorageRepository repository) : IFileStorageService
{
    private readonly IFileStorageRepository _repository = repository;

    public async Task<Result<string>> UploadTrackFileAsync(Stream stream, string fileName, string contentType)
    {
        if(stream == null)
        {
            return Errors.FileErrors.UploadError();
        }

        var key = $"tracks/{Guid.NewGuid()}";

        return await _repository.UploadFileAsync(key, stream, fileName, contentType);
    }

    public async Task<Result<string>> UploadImageFileAsync(Stream stream, string fileName, string contentType)
    {
        if (stream == null)
        {
            return Errors.FileErrors.UploadError();
        }

        var key = $"images/{Guid.NewGuid()}";

        return await _repository.UploadFileAsync(key, stream, fileName, contentType);
    }

    public async Task<Result<string>> GetEtagAsync(string key)
    {
        if(key == null)
        {
            return Errors.FileErrors.MissingKey();
        }

        var hash = await _repository.GetEtagFromFileAsync(key);
        return hash.Replace("\"", " ").Trim();
    }

    public async Task<Result<string>> DeleteFileAsync(string filePath)
    {
        if (filePath == null)
        {
            return Errors.FileErrors.MissingFilePath();
        }

        await _repository.DeleteFileAsync(filePath);

        return "Daleted";
    }

    public async Task<Result<string>> GetURLAsync(string filePath)
    {
        if (filePath == null)
        {
            return Errors.FileErrors.MissingFilePath();
        }

        return await _repository.GetUrlAsync(filePath);
    }
}

