namespace RiffBackend.Application.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null, string? name = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return Error.NotFound("record.not.found", $"{name ?? "record"} not found{forId}");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Validation("length.is.invalid", $"invalid{label}length");
        }

        public static Error AlreadyExist()
        {
            return Error.Validation("record.already.exist", "Record already exist");
        }

        public static Error Iternal()
        {
            return Error.Iternal("server.failure", "Server failure");
        }

        public static Error NotAllowed()
        {
            return Error.Iternal("not.allowed", "Not allowed");
        }
    }

}
