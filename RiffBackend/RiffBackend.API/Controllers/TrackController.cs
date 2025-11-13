using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Requests;
using RiffBackend.API.Responses;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using System.Security.Cryptography;

namespace RiffBackend.API.Controllers;

[ApiController]
[Route("api/track")]
public class TrackController : Controller
{
    private readonly ITrackService _service;
    private readonly IFileStorageService _fileService;
    public TrackController(ITrackService service, IFileStorageService fileService)
    {
        _service = service;
        _fileService = fileService;
    }

    [HttpGet("tracks")]
    public async Task<IActionResult> GetAllTracks()
    {
        var tracks = await _service.GetAllAsync();

        var response = tracks.Select(async t => new TrackResponse(t.Title, t.Author, t.TrackPath, await _fileService.GetURLAsync(t.ImagePath), t.CreatedAt));

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTrackById(Guid id)
    {
        //TO-DO валидация
        var track = await _service.GetById(id);

        var trackPath = await _fileService.GetURLAsync(track.TrackPath);
        var imagePath = await _fileService.GetURLAsync(track.ImagePath);

        return Ok(new TrackResponse(track.Title, track.Author, trackPath, imagePath, track.CreatedAt));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrack([FromForm] TrackRequest request)
    {
        using var trackStream = request.TrackFile.OpenReadStream();
        string trackPath = await _fileService.UploadTrackFileAsync(trackStream, request.TrackFile.FileName, request.TrackFile.ContentType);

        using var imageStream = request.TrackFile.OpenReadStream();
        string imagePath = await _fileService.UploadImageFileAsync(imageStream, request.ImageFile.FileName, request.ImageFile.ContentType);

        var track = Track.Create(Guid.NewGuid(), request.Title, trackPath, imagePath, request.Author, request.UserId);

        var id = await _service.AddAsync(track);

        return Created();
    }


    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateTrack(Guid id, [FromBody] TrackRequest request)
    {
        var oldTrack = await _service.GetById(id);
        var image = request.ImageFile;
        string imagePath = "";

        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var newHash = GetFileMd5(stream);

            var oldHash = await _fileService.GetEtagAsync(oldTrack.ImagePath);

            if (newHash != oldHash)
            {
                imagePath = await _fileService.UploadImageFileAsync(stream, image.FileName, image.ContentType);
            }
            else
            {
                imagePath = oldTrack.ImagePath;
            }
        }

        var trackFile = request.TrackFile;
        string trackPath = "";

        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var newHash = GetFileMd5(stream);

            var oldHash = await _fileService.GetEtagAsync(oldTrack.TrackPath);

            if (newHash != oldHash)
            {
                trackPath = await _fileService.UploadTrackFileAsync(stream, trackFile.FileName, trackFile.ContentType);
            }
            else
            {
                trackPath = oldTrack.TrackPath;
            }
        }

        var track = Track.Create(id, request.Title, trackPath, imagePath, request.Author, request.UserId);

        await _service.UpdateAsync(track);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTrack(Guid id)
    {
        var track = await _service.GetById(id);

        await _fileService.DeleteFileAsync(track.TrackPath);

        await _fileService.DeleteFileAsync(track.ImagePath);

        await _service.DeleteAsync(id);

        return NoContent();
    }

    private string GetFileMd5(Stream stream)
    {
        using (var md5 = MD5.Create())
        using (stream)
        {
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}

