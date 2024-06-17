using MediatR;

namespace Poc.BucketS3.Domain.Commands;

/// <summary>
///     Command for deleting an S3 bucket.
/// </summary>
/// <param name="bucketName">The name of the bucket to be deleted.</param>
public class DeleteBucketCommand(string bucketName) : BaseCommand(bucketName), IRequest
{
}