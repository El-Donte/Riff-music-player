namespace RiffBackend.Core.Abstraction.Service
{
    public interface IFileStorageService
    {
        Task DeleteFileAsync(string filePath);
        Task<string> GetURLAsync(string filePath);
        Task<string> UploadImageFileAsync(Stream stream, string fileName, string contentType);
        Task<string> UploadTrackFileAsync(Stream stream, string fileName, string contentType);
        Task<string> GetEtagAsync(string key);
    }
}