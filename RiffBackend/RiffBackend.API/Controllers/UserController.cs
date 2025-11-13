using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Requests;
using RiffBackend.API.Responses;
using RiffBackend.Core.Abstraction.Service;
using System.Security.Cryptography;

namespace RiffBackend.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly IUserService _service;
    private readonly IFileStorageService _fileService;

    public UserController(IUserService service, IFileStorageService fileStorage)
    {
        _service = service;
        _fileService = fileStorage;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _service.GetByIdAsync(id);

        var url = "";
        if(user.AvatarUrl != string.Empty || user.AvatarUrl != null)
        {
            url = await _fileService.GetURLAsync(user.AvatarUrl);
        }

        return Ok(new UserResponse(user.Id, user.Name, url));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromForm] UserRequest request)
    {
        //TO-DO так же валидации

        var image = request.AvatarImage;
        string imagePath = "";

        if (image != null) {
            using var stream = image.OpenReadStream();
            imagePath = await _fileService.UploadImageFileAsync(stream, image.FileName, image.ContentType);
        }

        var user = Core.Models.User.Create(Guid.NewGuid(), request.Name, request.Email, request.Password, imagePath);

        var id = await _service.AddAsync(user);

        return Created();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id,[FromForm] UserRequest request)
    {
        var oldUser = await _service.GetByIdAsync(id);
        var image = request.AvatarImage;
        string imagePath = "";

        if (image != null)
        {
            using var stream = image.OpenReadStream();
            var newHash = GetFileMd5(stream);

            var oldHash = await _fileService.GetEtagAsync(oldUser.AvatarUrl);

            if (newHash != oldHash)
            {
                imagePath = await _fileService.UploadImageFileAsync(stream, image.FileName, image.ContentType);
            }
            else
            {
                imagePath = oldUser.AvatarUrl;
            }
        }

        var user = Core.Models.User.Create(id, request.Name, request.Email, request.Password, imagePath);

        await _service.UpdateAsync(user);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
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
