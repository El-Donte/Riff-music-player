using System.Security.Cryptography;

namespace RiffBackend.Application.Extensions;

public static class StreamExtensions
{
    public static string ComputeMD5(this Stream stream)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}