using FluentValidation;
using Microsoft.AspNetCore.Http;
using RiffBackend.Application.Requests;
using RiffBackend.Core.Shared;

namespace RiffBackend.Application.Validators;

public sealed class TrackRequestValidator : AbstractValidator<TrackRequest>
{
    private readonly static string[] allowedImageTypes = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"];
    private readonly static string[] allowedTrackTypes = [".mp3", ".wav", ".ogg", ".flac", ".m4a", ".aac"];
    
    public TrackRequestValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("title").Serialize())
            .MaximumLength(30).WithMessage(Errors.General.ValueIsInvalidLength("title").Serialize());

        RuleFor(t => t.Author)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("author").Serialize())
            .MaximumLength(50).WithMessage(Errors.General.ValueIsInvalidLength("author").Serialize());

        RuleFor(t => t.UserId)
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("userId").Serialize());

        RuleFor(t => t.TrackFile)
            .Must(t => BeType(t,allowedTrackTypes)).WithMessage(Errors.FileErrors.InvalidType(allowedTrackTypes).Serialize())
            .Must(MaxLength).WithMessage(Errors.FileErrors.InvalidSize(50).Serialize());

        RuleFor(t => t.ImageFile)
            .Must(t => BeType(t, allowedImageTypes)).WithMessage(Errors.FileErrors.InvalidType(allowedImageTypes).Serialize())
            .Must(MaxLength).WithMessage(Errors.FileErrors.InvalidSize(50).Serialize());
    }

    private static bool BeType(IFormFile file, string[] types)
    {
        if(file == null) return true;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (types.Contains(extension))
            return true;

        return false;
    }

    private static bool MaxLength(IFormFile file)
    {
        if (file == null) return true;
        return file.Length < 50 * 1024 * 1024;
    }
}

