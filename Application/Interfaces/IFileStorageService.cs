namespace Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream content, string contentType, string bucketName, string objectName,
        CancellationToken ct = default);
}