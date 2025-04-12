namespace SeconAPI.Application.Interfaces.Services;

public interface IStorageService
{
    Task<string> UploadFileAsync(string bucketName, string objectName, Stream data, string contentType);
    Task<Stream> GetFileAsync(string bucketName, string objectName);
    Task<bool> DoesObjectExistAsync(string bucketName, string objectName);
    Task CreateBucketIfNotExistsAsync(string bucketName);
    Task<string> GetPresignedUrlAsync(string bucketName, string objectName, int expiryInMinutes = 60);
}