using MediatR;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Commands;

/// <summary>
///     Command for retrieving all files from an S3 bucket with optional filtering and pagination.
/// </summary>
/// <param name="bucketName">The name of the bucket containing the files to be retrieved.</param>
/// <param name="maxKeys">The maximum number of files to retrieve.</param>
/// <param name="prefix">Optional prefix to filter the files by key.</param>
/// <param name="continuationToken">Optional continuation token for pagination.</param>
public class GetAllFileCommand(
    string bucketName,
    int maxKeys,
    string? prefix = default,
    string? continuationToken = default) : BaseCommand(bucketName), IRequest<(List<FileModel>, string)>
{
    /// <summary>
    ///     Gets the maximum number of files to retrieve.
    /// </summary>
    public int MaxKeys { get; } = maxKeys;

    /// <summary>
    ///     Gets the optional prefix to filter the files by key.
    /// </summary>
    public string? Prefix { get; } = prefix;

    /// <summary>
    ///     Gets the optional continuation token for pagination.
    /// </summary>
    public string? ContinuationToken { get; } = continuationToken;
}