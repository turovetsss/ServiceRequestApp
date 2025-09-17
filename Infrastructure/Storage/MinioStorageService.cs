using Application.Interfaces;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Storage;

public class MinioStorageService : IFileStorageService
{
    private readonly IMinioClient client;
    private readonly MinioOptions options;

    public MinioStorageService(IOptions<MinioOptions> options)
    {
        this.options = options.Value;
        client = new MinioClient()
            .WithEndpoint(this.options.Endpoint)
            .WithCredentials(this.options.AccessKey, this.options.SecretKey)
            .WithSSL(this.options.WithSSL)
            .Build();
    }

    public async Task<string> UploadAsync(Stream content, string contentType, string bucketName, string objectName,
        CancellationToken ct = default)
    {
        await EnsureBucketExistsAsync(bucketName, ct);

        await client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(content)
            .WithObjectSize(content.Length)
            .WithContentType(contentType), ct);

        var endpoint = string.IsNullOrWhiteSpace(options.PublicEndpoint) ? options.Endpoint : options.PublicEndpoint;
        var scheme = options.WithSSL ? "https" : "http";
        return $"{scheme}://{endpoint}/{bucketName}/{objectName}";
    }

    public async Task EnsureBucketExistsAsync(string bucketName, CancellationToken ct = default)
    {
        var exists = await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), ct);
        if (!exists) await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName), ct);
    }

    /*public async Task DeleteAsync(string bucketName, string objectName, CancellationToken ct = default)
    {
        await client.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(bucketName).WithObject(objectName), ct);
    }*/
}