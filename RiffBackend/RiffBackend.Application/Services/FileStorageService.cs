using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;

namespace RiffBackend.Application.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IFileStorageRepository _repository;

    public FileStorageService(IFileStorageRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> UploadTrackFileAsync(Stream stream, string fileName, string contentType)
    {
        //TO-DO валидация

        var key = $"tracks/{Guid.NewGuid()}";

        return await _repository.UploadFileAsync(key, stream, fileName, contentType);
    }

    public async Task<string> UploadImageFileAsync(Stream stream, string fileName, string contentType)
    {
        //TO-DO валидация

        var key = $"images/{Guid.NewGuid()}";

        return await _repository.UploadFileAsync(key, stream, fileName, contentType);
    }

    public async Task DeleteFileAsync(string filePath)
    {
        await _repository.DeleteFileAsync(filePath);
    }

    public async Task<string> GetURLAsync(string filePath)
    {
        return await _repository.GetUrlAsync(filePath);
    }
}

