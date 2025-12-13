using RiffBackend.Core.Shared;

namespace RiffBackend.Core.Abstraction.Service;

public interface IFileStorageService
{
    Task<Result<string>> DeleteFileAsync(string filePath, CancellationToken ct = default);
    Task<Result<string>> GetURLAsync(string filePath, CancellationToken ct = default);
    Task<Result<string>> UploadImageFileAsync(Stream stream, string fileName, 
        string contentType, CancellationToken ct = default);
    Task<Result<string>> UploadTrackFileAsync(Stream stream, string fileName, 
        string contentType, CancellationToken ct = default);
    Task<Result<string>> GetEtagAsync(string key, CancellationToken ct = default);
}
