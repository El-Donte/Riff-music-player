namespace RiffBackend.Core.Abstraction.Repository;

public interface IFileStorageRepository
{
    Task DeleteFileAsync(string key, CancellationToken ct = default);
    Task<string> GetUrlAsync(string key, CancellationToken ct = default);
    Task<string> UploadFileAsync(string key, Stream stream, 
        string fileName, string contentType, CancellationToken ct = default);
    Task<string> GetEtagFromFileAsync(string key, CancellationToken ct = default);
}
