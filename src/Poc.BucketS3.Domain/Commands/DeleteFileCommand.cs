using MediatR;

namespace Poc.BucketS3.Domain.Commands;

/// <summary>
///     Command for deleting a file from an S3 bucket.
/// </summary>
/// <param name="bucketName">The name of the bucket containing the file to be deleted.</param>
/// <param name="key">The key of the file to be deleted.</param>
public class DeleteFileCommand(string bucketName, string key) : BaseCommand(bucketName, key), IRequest
{
}