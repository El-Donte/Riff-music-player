namespace RiffBackend.Application.Shared;

public record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error Validation(string code, string message) =>
        new Error(code, message, ErrorType.Validation);

    public static Error NotFound(string code, string message) =>
        new Error(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) => 
        new Error(code, message, ErrorType.Conflict);

    public static Error Iternal(string code, string message) =>
        new Error(code, message, ErrorType.Internal);

    public static Error None() =>
        new Error("", "", ErrorType.None);
}


public enum ErrorType
{
    None,
    Validation,
    NotFound,
    Conflict,
    Internal
}
