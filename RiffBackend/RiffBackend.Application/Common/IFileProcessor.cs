using Microsoft.AspNetCore.Http;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Common
{
    public interface IFileProcessor
    {
        Task<Error> EnrichWithUrlsAsync(Track track, CancellationToken ct);
        Task<Result<string>> UploadNewOrKeepOldAsync(IFormFile? file, string oldPath, CancellationToken ct,
            Func<Stream, string, string, CancellationToken, Task<Result<string>>> uploadFunc);
    }
}