namespace Poc.BucketS3.Domain.Commands;

/// <summary>
///     Base command class containing common properties for S3 bucket operations.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="BaseCommand" /> class.
/// </remarks>
/// <param name="bucketName">The name of the S3 bucket.</param>
/// <param name="key">The key for the S3 object.</param>
public class BaseCommand(string? bucketName = default, string? key = default)
{
    /// <summary>
    ///     Gets or sets the name of the S3 bucket.
    /// </summary>
    public string? BucketName { get; init; } = bucketName;

    /// <summary>
    ///     Gets or sets the key for the S3 object.
    /// </summary>
    public string? Key { get; init; } = key;
}