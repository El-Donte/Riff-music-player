using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Extensions;
using RiffBackend.API.Responses;
using RiffBackend.Application.Requests;
using RiffBackend.Core.Abstraction.Service;

namespace RiffBackend.API.Controllers;

[ApiController]
[RequestSizeLimit(100 * 1024 * 1024)]
[Route("api/track")]
public class TrackController(ITrackService service, IValidator<TrackRequest> validator) : Controller
{
    private readonly ITrackService _service = service;
    private readonly IValidator<TrackRequest> _validator = validator;

    
    [HttpGet("/api/tracks")]
    public async Task<IActionResult> GetAllTracks(CancellationToken ct = default)
    {
        var result = await _service.GetAllAsync(ct);

        return result.ToActionResult(tracks =>
                    Ok(Envelope.Ok(tracks.Select(t =>
                        new TrackResponse(t.Id, t.Title, t.Author, t.TrackPath, t.ImagePath, t.CreatedAt)))));
    }

    [Authorize]
    [HttpGet("/api/tracks/{title}")]
    public async Task<IActionResult> GetAllTracksByTitle(string title, CancellationToken ct = default)
    {
        var result = await _service.GetAllByTitleAsync(title, ct);

        return result.ToActionResult(tracks =>
                    Ok(Envelope.Ok(tracks.Select(t => 
                        new TrackResponse(t.Id, t.Title, t.Author, t.TrackPath, t.ImagePath, t.CreatedAt)))));
    }

    [Authorize]
    [HttpGet("/api/tracks/{id:guid}")]
    public async Task<IActionResult> GetAllTracksByUserId(Guid id, CancellationToken ct = default)
    {
        var result = await _service.GetAllByUserIdAsync(id, ct);

        return result.ToActionResult(tracks =>
                    Ok(Envelope.Ok(tracks.Select(t => 
                        new TrackResponse(t.Id, t.Title, t.Author, t.TrackPath, t.ImagePath, t.CreatedAt)))));
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTrackById(Guid id, CancellationToken ct = default)
    {
        var result = await _service.GetById(id, ct);

        return result.ToActionResult(track =>
                    Ok(Envelope.Ok(new TrackResponse(track.Id, track.Title, 
                        track.Author, track.TrackPath, track.ImagePath, track.CreatedAt))));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTrack([FromForm] TrackRequest request, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.AddAsync(Guid.NewGuid(), request.Title, request.Author, 
            request.UserId, request.ImageFile, request.TrackFile, ct);

        return result.ToActionResult(id => Ok(Envelope.Ok(id)));
    }

    [Authorize]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateTrack(Guid id, [FromForm] TrackRequest request, 
        CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }

        var result = await _service.UpdateAsync(id, request.Title, request.Author, 
            request.UserId, request.ImageFile, request.TrackFile, ct);

        return result.ToActionResult(id => Ok(Envelope.Ok(id)));
    }

    [Authorize]
    [HttpPost("like")]
    public async Task<IActionResult> LikeTrack([FromBody] LikeTrackRequest request, CancellationToken ct = default)
    {
        var result = await _service.LikeTrackAsync(request.UserId, request.TrackId, ct);

        return result.ToActionResult(id => Ok(Envelope.Ok(id)));
    }

    [Authorize]
    [HttpGet("like/{userId:guid}")]
    public async Task<IActionResult> GetAllLikedTracksByUserId(Guid userId, CancellationToken ct = default)
    {
        var result = await _service.GetLikedTracksAsync(userId, ct);

        return result.ToActionResult(tracks => 
            Ok(Envelope.Ok(tracks.Select(t => 
                new TrackResponse(t.Id, t.Title, t.Author, t.TrackPath, t.ImagePath, t.CreatedAt)))));
    }

    [Authorize]
    [HttpPut("like")]
    public async Task<IActionResult> GetIsLiked([FromBody] LikeTrackRequest request, CancellationToken ct = default)
    {
        var result = await _service.IsLikedAsync(request.UserId, request.TrackId, ct);

        return result.ToActionResult(isLiked => Ok(Envelope.Ok(isLiked)));
    }

    [Authorize]
    [HttpDelete("like")]
    public async Task<IActionResult> UnlikeTrack([FromBody] LikeTrackRequest request, CancellationToken ct = default)
    {
        var result = await _service.UnlikeTrackAsync(request.UserId, request.TrackId, ct);

        return result.ToActionResult(id => Ok(Envelope.Ok(id)));
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTrack(Guid id, CancellationToken ct = default)
    {
        var result = await _service.DeleteAsync(id, ct);

        return result.ToActionResult(id => Ok(Envelope.Ok(id)));
    }
}