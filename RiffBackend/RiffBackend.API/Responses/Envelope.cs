using RiffBackend.Core.Shared;

namespace RiffBackend.API.Responses;

public record ResponseError(string Code, string Message, string? InvalidField);

public record Envelope
{
    public object? Result { get; }

    public List<ResponseError>? Errors { get; }

    public DateTime TimeGenerated { get; }

    private Envelope(object? result, List<ResponseError>? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Error(List<ResponseError> errors) =>
        new(null, errors);

    public static Envelope Error(Error error) =>
        new(null, [new ResponseError(error.Code, error.Message, default)]);
}
