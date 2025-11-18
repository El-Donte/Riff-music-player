namespace RiffBackend.Core.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
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

    public static class UserErrors
    {
        public static Error MissingId()
        {
            return Error.Validation("id.missing", "User id is missing");
        }

        public static Error EmailDuplicate(string? email = null)
        {
            var forEmail = email == null ? "" : $"{email}";
            return Error.Conflict("email.already.used", $"this {forEmail} is already used");
        }

        public static Error NotFound(Guid? id = null, string? email = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            var forEmal = email == null ? "" : $"for email '{email}'";
            return Error.NotFound("user.not.found", $"User not found{forId}{forEmal}");
        }

        public static Error IncorrectPassword()
        {
            return Error.Validation("password.incorrect", "Password incorrect");
        }
    }

    public static class TrackErrors
    {
        public static Error MissingId()
        {
            return Error.Validation("id.missing", "Track id is missing");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return Error.NotFound("track.not.found", $"track not found{forId}");
        }
    }

    public static class FileErrors
    {
        public static Error MissingKey()
        {
            return Error.Validation("key.missing", "File key is missing");
        }

        public static Error MissingFilePath()
        {
            return Error.Validation("filePath.missing", "File path is missing");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return Error.NotFound("track.not.found", $"track not found{forId}");
        }

        public static Error UploadError()
        {
            return Error.Iternal("upload.faild", "Something went wrong while uploading file");
        }

        public static Error MissingFile()
        {
            return Error.Validation("file.null", "File is missing or null");
        }
    }
}
