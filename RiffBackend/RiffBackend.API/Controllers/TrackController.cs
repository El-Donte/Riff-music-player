using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Extensions;
using RiffBackend.API.Responses;
using RiffBackend.Application.Requests;
using RiffBackend.Core.Abstraction.Service;

namespace RiffBackend.API.Controllers;

[ApiController]
[Authorize]
[Route("api/track")]
public class TrackController(ITrackService service, IValidator<TrackRequest> validator) : Controller
{
    private readonly ITrackService _service = service;
    private readonly IValidator<TrackRequest> _validator = validator;

    [HttpGet("tracks")]
    public async Task<IActionResult> GetAllTracks()
    {
        var result = await _service.GetAllAsync();

        return result.ToActionResult(tracks =>
                    Ok(Envelope.Ok(tracks.Select(t => new TrackResponse(t.Title, t.Author, t.TrackPath, t.ImagePath, t.CreatedAt)))));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTrackById(Guid id)
    {
        var result = await _service.GetById(id);

        return result.ToActionResult(track =>
                    Ok(Envelope.Ok(new TrackResponse(track.Title, track.Author, track.TrackPath, track.ImagePath, track.CreatedAt))));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrack([FromForm] TrackRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.AddAsync(Guid.NewGuid(), request.Title, request.Author, request.UserId, request.ImageFile, request.TrackFile);

        return result.ToActionResult(track => Ok(Envelope.Ok()));
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateTrack(Guid id, [FromBody] TrackRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.UpdateAsync(id, request.Title, request.Author, request.UserId, request.ImageFile, request.TrackFile);

        return result.ToActionResult(track => Ok(Envelope.Ok()));
    }

    [HttpPost("/like")]
    public async Task<IActionResult> LikeTrack([FromBody] LikeTrackRequest request)
    {
        var result = await _service.LikeTrackAsync(request.UserId, request.TrackId);

        return result.ToActionResult(track => Ok(Envelope.Ok()));
    }

    [HttpDelete("/like")]
    public async Task<IActionResult> UnlikeTrack([FromBody] LikeTrackRequest request)
    {
        var result = await _service.UnlikeTrackAsync(request.UserId, request.TrackId);

        return result.ToActionResult(track => Ok(Envelope.Ok()));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTrack(Guid id)
    {
        var result = await _service.DeleteAsync(id);

        return result.ToActionResult(track => NoContent());
    }
}

