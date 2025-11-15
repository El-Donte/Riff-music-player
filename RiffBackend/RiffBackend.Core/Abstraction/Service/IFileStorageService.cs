using RiffBackend.Core.Shared;

namespace RiffBackend.Core.Abstraction.Service
{
    public interface IFileStorageService
    {
        Task<Result<string>> DeleteFileAsync(string filePath);
        Task<Result<string>> GetURLAsync(string filePath);
        Task<Result<string>> UploadImageFileAsync(Stream stream, string fileName, string contentType);
        Task<Result<string>> UploadTrackFileAsync(Stream stream, string fileName, string contentType);
        Task<Result<string>> GetEtagAsync(string key);
    }
}