namespace RiffBackend.Core.Abstraction.Repository
{
    public interface IFileStorageRepository
    {
        Task DeleteFileAsync(string key);
        Task<string> GetUrlAsync(string key);
        Task<string> UploadFileAsync(string key, Stream stream, string fileName, string contentType);
    }
}