using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Extensions;
using RiffBackend.API.Requests;
using RiffBackend.API.Responses;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using RiffBackend.Core.Shared;

namespace RiffBackend.API.Controllers;

[ApiController]
[Authorize]
[Route("api/track")]
public class TrackController : Controller
{
    private readonly ITrackService _service;

    public TrackController(ITrackService service)
    {
        _service = service;
    }

    [HttpGet("tracks")]
    public async Task<IActionResult> GetAllTracks()
    {
        var result = await _service.GetAllAsync();

        return result.ToActionResult(tracks => Ok(tracks.Select(t => new TrackResponse(t.Title, t.Author, t.TrackPath, t.ImagePath, t.CreatedAt))));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTrackById(Guid id)
    {
        var result = await _service.GetById(id);

        return result.ToActionResult(track => Ok(new TrackResponse(track.Title, track.Author, track.TrackPath, track.ImagePath, track.CreatedAt)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrack([FromForm] TrackRequest request)
    {
        var trackFile = request.TrackFile;
        var imageFile = request.ImageFile;

        if (trackFile is null || imageFile is null)
        {
            return BadRequest(Errors.FileErrors.MissingFile());
        }

        var track = Track.Create(Guid.NewGuid(), request.Title, request.Author, request.UserId);
        using var trackStream = request.TrackFile.OpenReadStream();
        using var imageStream = request.TrackFile.OpenReadStream();

        var result = await _service.AddAsync(track, trackStream, trackFile.FileName, trackFile.ContentType, 
                                                    imageStream, imageFile.FileName, imageFile.ContentType);

        return result.ToActionResult(track => Ok());
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateTrack(Guid id, [FromBody] TrackRequest request)
    {
        var imageFile = request.ImageFile;
        var trackFile = request.TrackFile;

        if (imageFile is null || trackFile is null)
        {
            return BadRequest(Errors.FileErrors.MissingFile());
        }

        var track = Track.Create(id, request.Title, request.Author, request.UserId);
        using var trackStream = trackFile.OpenReadStream();
        using var imageStream = imageFile.OpenReadStream();

        var result = await _service.UpdateAsync(track, trackStream, trackFile.FileName, trackFile.ContentType, 
                                          imageStream, imageFile.FileName, imageFile.ContentType);

        return result.ToActionResult(track => Ok());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTrack(Guid id)
    {
        var result = await _service.DeleteAsync(id);

        return result.ToActionResult(track => NoContent());
    }
}

