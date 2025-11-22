using Microsoft.AspNetCore.Http;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Common
{
    public interface IFileProcessor
    {
        Task<Error> EnrichWithUrlsAsync(Track track);
        Task<Result<string>> UploadNewOrKeepOldAsync(IFormFile? file, string oldPath, Func<Stream, string, string, Task<Result<string>>> uploadFunc);
    }
}