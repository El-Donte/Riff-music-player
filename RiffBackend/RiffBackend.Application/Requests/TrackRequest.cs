using Microsoft.AspNetCore.Http;

namespace RiffBackend.Application.Requests;

public record TrackRequest(string Title, string Author, Guid UserId, IFormFile? TrackFile, IFormFile? ImageFile);
