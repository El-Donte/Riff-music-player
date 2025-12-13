using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using RiffBackend.Core.Abstraction.Repository;

namespace RiffBackend.Infrastructure.Repositories;

public class FileStorageRepository(IAmazonS3 s3Client, IOptions<S3Settings> s3Options) : IFileStorageRepository
{
    private readonly IAmazonS3 _s3Client = s3Client;
    private readonly string _bucketName = s3Options.Value.BucketName ?? throw new ArgumentNullException("BucketName is apsent");

    public async Task<string> UploadFileAsync(string key, Stream stream, 
        string fileName, string contentType, CancellationToken ct = default)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            InputStream = stream,
            Key = key,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(putRequest, ct);

        return key;
    }

    public async Task<string> GetUrlAsync(string key, CancellationToken ct = default)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddHours(12)
        };

        return await _s3Client.GetPreSignedURLAsync(request);
    }

    public async Task<string> GetEtagFromFileAsync(string key, CancellationToken ct = default)
    {
        var response = await _s3Client.GetObjectMetadataAsync(_bucketName, key, ct);

        return response.ETag;
    }

    public async Task DeleteFileAsync(string key, CancellationToken ct = default)
    {
        await _s3Client.DeleteObjectAsync(_bucketName, key, ct);
    }
}
