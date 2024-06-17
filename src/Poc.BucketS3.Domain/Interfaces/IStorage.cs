using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Interfaces;

/// <summary>
///     Interface for storage operations in S3.
/// </summary>
public interface IStorage
{
    /// <summary>
    ///     Creates a new bucket in S3.
    /// </summary>
    /// <param name="bucketName">The name of the bucket to create.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateBucketAsync(string bucketName, CancellationToken cancellationToken);

    /// <summary>
    ///     Checks if a bucket exists in S3.
    /// </summary>
    /// <param name="bucketName">The name of the bucket to check.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation, containing a boolean indicating if the bucket exists.</returns>
    Task<bool> DoesS3BucketExistAsync(string bucketName, CancellationToken cancellationToken);

    /// <summary>
    ///     Lists all buckets in S3.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of bucket models.</returns>
    Task<List<BucketModel>> ListBucketsAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a bucket from S3.
    /// </summary>
    /// <param name="bucketName">The name of the bucket to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteBucketAsync(string bucketName, CancellationToken cancellationToken);

    /// <summary>
    ///     Uploads a file to S3.
    /// </summary>
    /// <param name="file">The file model containing the file to upload.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UploadFileAsync(FileModel file, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieves a file from S3.
    /// </summary>
    /// <param name="file">The file model containing the file to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation, containing the file model.</returns>
    Task<FileModel> GetFileAsync(FileModel file, CancellationToken cancellationToken);

    /// <summary>
    ///     Deletes a file from S3.
    /// </summary>
    /// <param name="fileModel">The file model containing the file to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteFileAsync(FileModel fileModel, CancellationToken cancellationToken);

    /// <summary>
    ///     Lists files in an S3 bucket with optional filtering and pagination.
    /// </summary>
    /// <param name="fileModel">The file model containing the bucket information.</param>
    /// <param name="maxKeys">The maximum number of files to list.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <param name="continuationToken">Optional continuation token for pagination.</param>
    /// <param name="prefix">Optional prefix to filter the files by key.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing a tuple with a list of file models and a
    ///     continuation token.
    /// </returns>
    Task<(List<FileModel>, string)> ListFilesAsync(FileModel fileModel, int maxKeys,
        CancellationToken cancellationToken, string? continuationToken = default, string? prefix = default);

    /// <summary>
    ///     Retrieves a pre-signed URL for a file in S3.
    /// </summary>
    /// <param name="fileModel">The file model containing the file information.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation, containing the file model with the pre-signed URL.</returns>
    Task<FileModel> GetUrlFileAsync(FileModel fileModel, CancellationToken cancellationToken);
}