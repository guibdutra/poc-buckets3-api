using MediatR;

namespace Poc.BucketS3.Domain.Commands;

/// <summary>
///     Command for creating a new S3 bucket.
/// </summary>
/// <param name="bucketName">The name of the bucket to be created.</param>
public class CreateBucketCommand(string bucketName) : BaseCommand(bucketName), IRequest
{
}