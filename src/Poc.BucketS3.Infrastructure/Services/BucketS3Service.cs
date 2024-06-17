using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;
using Poc.BucketS3.Infrastructure.Exceptions;

namespace Poc.BucketS3.Infrastructure.Services;

/// <summary>
///     Service class for interacting with S3 buckets and files.
/// </summary>
public class BucketS3Service : IStorage
{
    private readonly IAmazonS3 _amazonS3;
    private readonly ITransferUtility _transferUtility;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BucketS3Service" /> class.
    /// </summary>
    /// <param name="amazonS3">The Amazon S3 client.</param>
    /// <param name="transferUtility">The transfer utility for uploading files.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="amazonS3" /> or <paramref name="transferUtility" />
    ///     is null.
    /// </exception>
    public BucketS3Service(IAmazonS3 amazonS3, ITransferUtility transferUtility)
    {
        _amazonS3 = amazonS3 ?? throw new ArgumentNullException(nameof(amazonS3));
        _transferUtility = transferUtility ?? throw new ArgumentNullException(nameof(transferUtility));
    }

    /// <inheritdoc />
    public async Task CreateBucketAsync(string bucketName, CancellationToken cancellationToken)
    {
        var bucketExists = await DoesS3BucketExistAsync(bucketName, cancellationToken);
        if (bucketExists)
            throw new ConflictException($"Bucket {bucketName} already exists.");

        await _amazonS3.PutBucketAsync(bucketName, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DoesS3BucketExistAsync(string bucketName, CancellationToken cancellationToken)
    {
        var response = await _amazonS3.ListBucketsAsync(cancellationToken);
        return response.Buckets.Any(b => b.BucketName == bucketName);
    }

    /// <inheritdoc />
    public async Task<List<BucketModel>> ListBucketsAsync(CancellationToken cancellationToken)
    {
        var response = await _amazonS3.ListBucketsAsync(cancellationToken);

        return response.Buckets
            .Select(bucket => new BucketModel(bucket.BucketName))
            .ToList();
    }

    /// <inheritdoc />
    public async Task DeleteBucketAsync(string bucketName, CancellationToken cancellationToken)
    {
        await DeleteAllObjectsAsync(bucketName, cancellationToken);

        var deleteBucketRequest = new DeleteBucketRequest
        {
            BucketName = bucketName
        };

        await _amazonS3.DeleteBucketAsync(deleteBucketRequest, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UploadFileAsync(FileModel file, CancellationToken cancellationToken)
    {
        TransferUtilityUploadRequest request = new()
        {
            BucketName = file.BucketName,
            Key = file.Key,
            InputStream = file.Stream,
            ContentType = file.ContentType
        };

        await _transferUtility.UploadAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<FileModel> GetFileAsync(FileModel file, CancellationToken cancellationToken)
    {
        try
        {
            var s3Object = await _amazonS3.GetObjectAsync(file.BucketName, file.Key, cancellationToken);
            file.SetStream(s3Object.ResponseStream);
            file.SetContentType(s3Object.Headers.ContentType);
            return file;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task DeleteFileAsync(FileModel fileModel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(fileModel);
        if (string.IsNullOrEmpty(fileModel.Key))
            throw new ArgumentException("File key cannot be null or empty", nameof(fileModel.Key));

        DeleteObjectRequest request = new()
        {
            BucketName = fileModel.BucketName,
            Key = fileModel.Key
        };

        var response = await _amazonS3.DeleteObjectAsync(request, cancellationToken);

        if (response.HttpStatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException("The file was not found or an error occurred.");
    }

    /// <inheritdoc />
    public async Task<(List<FileModel>, string)> ListFilesAsync(
        FileModel fileModel,
        int maxKeys,
        CancellationToken cancellationToken,
        string? continuationToken = default,
        string? prefix = default)
    {
        ListObjectsV2Request request = new()
        {
            BucketName = fileModel.BucketName,
            MaxKeys = maxKeys,
            Prefix = prefix,
            ContinuationToken = continuationToken
        };

        var response = await _amazonS3.ListObjectsV2Async(request, cancellationToken);

        if (response.HttpStatusCode == HttpStatusCode.NotFound)
            throw new NotFoundException("The file was not found or an error occurred.");

        var listObjects =
            response.S3Objects.Select(item => new FileModel(item.BucketName!, item.Key)).ToList();

        return (listObjects, response.NextContinuationToken);
    }

    /// <inheritdoc />
    public Task<FileModel> GetUrlFileAsync(FileModel fileModel, CancellationToken cancellationToken)
    {
        try
        {
            const int preSignedUrlExpires = 1;
            var request = new GetPreSignedUrlRequest
            {
                BucketName = fileModel.BucketName,
                Key = fileModel.Key,
                Expires = DateTime.Now.AddHours(preSignedUrlExpires)
            };

            var preSignedUrl = _amazonS3.GetPreSignedURL(request);
            return Task.FromResult(new FileModel(fileModel.BucketName, fileModel.Key!, preSignedUrl, request.Expires));
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(ex.Message);
        }
    }

    #region Private Methods

    /// <summary>
    ///     Deletes all objects in a specified S3 bucket.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    private async Task DeleteAllObjectsAsync(string bucketName, CancellationToken cancellationToken)
    {
        var bucketExists = await DoesS3BucketExistAsync(bucketName, cancellationToken);
        if (!bucketExists)
            throw new BadRequestException($"The bucket {bucketName} does not exist.");

        ListObjectsV2Request listRequest = new()
        {
            BucketName = bucketName
        };

        ListObjectsV2Response listResponse;
        do
        {
            listResponse = await _amazonS3.ListObjectsV2Async(listRequest, cancellationToken);

            if (listResponse.S3Objects.Count > 0)
            {
                var deleteRequest = new DeleteObjectsRequest
                {
                    BucketName = bucketName,
                    Objects = []
                };

                foreach (var obj in listResponse.S3Objects) deleteRequest.Objects.Add(new KeyVersion { Key = obj.Key });

                await _amazonS3.DeleteObjectsAsync(deleteRequest, cancellationToken);
            }

            listRequest.ContinuationToken = listResponse.NextContinuationToken;
        } while (listResponse.IsTruncated);
    }

    #endregion
}