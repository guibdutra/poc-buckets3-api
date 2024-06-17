namespace Poc.BucketS3.Domain.Models;

/// <summary>
///     Model representing an S3 bucket.
/// </summary>
/// <param name="bucketName">The name of the S3 bucket.</param>
public class BucketModel(string bucketName) : BaseModel(bucketName)
{
}