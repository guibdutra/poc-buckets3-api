namespace Poc.BucketS3.Domain.Models;

/// <summary>
///     Base model class containing common properties for S3 bucket operations.
/// </summary>
/// <param name="bucketName">The name of the S3 bucket.</param>
/// <remarks>
///     Initializes a new instance of the <see cref="BaseModel" /> class.
/// </remarks>
/// <param name="bucketName">The name of the S3 bucket.</param>
public class BaseModel(string bucketName)
{
    /// <summary>
    ///     Gets the name of the S3 bucket.
    /// </summary>
    public string BucketName { get; } = bucketName;
}