using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using SeconAPI.Application.Interfaces.Services;

namespace SeconAPI.Application.Services;

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioStorageService> _logger;

    public MinioStorageService(IMinioClient minioClient, ILogger<MinioStorageService> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task CreateBucketIfNotExistsAsync(string bucketName)
    {
        try
        {
            var beArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
            
            bool found = await _minioClient.BucketExistsAsync(beArgs);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
                
                await _minioClient.MakeBucketAsync(mbArgs);
                _logger.LogInformation($"Created bucket {bucketName}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating bucket {bucketName}");
            throw;
        }
    }

    public async Task<bool> DoesObjectExistAsync(string bucketName, string objectName)
    {
        try
        {
            var statArgs = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);
            
            await _minioClient.StatObjectAsync(statArgs);
            return true;
        }
        catch (ObjectNotFoundException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error checking if object {objectName} exists in bucket {bucketName}");
            throw;
        }
    }

    public async Task<Stream> GetFileAsync(string bucketName, string objectName)
    {
        try
        {
            var memoryStream = new MemoryStream();
            var getArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                });
            
            await _minioClient.GetObjectAsync(getArgs);
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting object {objectName} from bucket {bucketName}");
            throw;
        }
    }

    public async Task<string> GetPresignedUrlAsync(string bucketName, string objectName, int expiryInMinutes = 60)
    {
        try
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithExpiry(expiryInMinutes * 60);
            
            return await _minioClient.PresignedGetObjectAsync(args);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error generating presigned URL for {objectName} in bucket {bucketName}");
            throw;
        }
    }

    public async Task<string> UploadFileAsync(string bucketName, string objectName, Stream data, string contentType)
    {
        try
        {
            await CreateBucketIfNotExistsAsync(bucketName);
            
            var putArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(data.Length)
                .WithContentType(contentType);
            
            await _minioClient.PutObjectAsync(putArgs);
            
            return objectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error uploading object {objectName} to bucket {bucketName}");
            throw;
        }
    }
}