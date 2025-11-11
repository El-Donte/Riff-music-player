using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Requests;
using RiffBackend.API.Responses;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Core.Models;
using System.IO;
using System.Net.Mime;

namespace RiffBackend.API.Controllers;

[ApiController]
[Route("api/track")]
public class TrackController : Controller
{
    private readonly ITrackService _service;
    private readonly IAmazonS3 _s3client;
    public TrackController(ITrackService service, IAmazonS3 s3)
    {
        _service = service;
        _s3client = s3;
    }

    [HttpGet("tracks")]
    public async Task<IActionResult> GetAllTracks()
    {
        var tracks = await _service.GetAllAsync();

        var response = tracks.Select(t => new TrackResponse(t.Title, t.Author, t.TrackPath, t.ImagePath, t.CreatedAt));

        return Ok(response);
    }

    [HttpGet("track-file{key:}")]
    public async Task<IActionResult> GetTrackFile(string key)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = "riff-backet",
            Key = $"tracks/{key}"
        };

        var response = await _s3client.GetObjectAsync(getRequest);
        return File(response.ResponseStream, response.Headers.ContentType);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTrackById(Guid id)
    {
        //TO-DO валидация
        var track = await _service.GetById(id);

        return Ok(new TrackResponse(track.Title, track.Author, track.TrackPath, track.ImagePath, track.CreatedAt));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrack([FromForm] TrackRequest request)
    {
        var file = request.file;
        //TO-DO валидация
        using var stream = file.OpenReadStream();

        var key = $"tracks/{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

        var putRequest = new PutObjectRequest
        {
            InputStream = stream,
            Key = key,
            BucketName = "riff-backet",
            ContentType = file.ContentType,
            AutoCloseStream = false
        };

        await _s3client.PutObjectAsync(putRequest);



        var track = Track.Create(Guid.NewGuid(), request.Title, request.TrackPath, request.ImagePath, request.Author, request.UserId, null, null);

        var id = await _service.AddAsync(track);

        return Created();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateTrack(Guid id,[FromBody] TrackRequest request)
    {
        var track = Track.Create(id, request.Title, request.TrackPath, request.ImagePath, request.Author, request.UserId, null, null);

        await _service.UpdateAsync(track);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTrack(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}

