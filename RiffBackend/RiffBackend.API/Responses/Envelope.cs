using RiffBackend.Core.Shared;

namespace RiffBackend.API.Responses;

public record Envelope
{
    public object? Result { get; }

    public List<Error>? Errors { get; }

    public DateTime TimeGenerated { get; }

    private Envelope(object? result, List<Error>? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Error(List<Error> errors) =>
        new(null, errors);

    public static Envelope Error(Error error) =>
        new(null, new List<Error>() { error });
}
