namespace Poc.BucketS3.Domain.Models;

/// <summary>
///     Model representing a file in an S3 bucket.
/// </summary>
public class FileModel : BaseModel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FileModel" /> class with the specified bucket name.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    public FileModel(string bucketName) : base(bucketName)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileModel" /> class with the specified bucket name and file name.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="fileName">The name of the file.</param>
    public FileModel(string bucketName, string fileName) : base(bucketName)
    {
        Key = fileName;
        FileName = fileName;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileModel" /> class with the specified parameters.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="key">The key of the file in the bucket.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="stream">The file stream.</param>
    /// <param name="contentType">The content type of the file.</param>
    public FileModel(
        string bucketName,
        string key,
        string fileName,
        Stream stream,
        string contentType) : base(bucketName)
    {
        Key = key;
        FileName = fileName;
        Stream = stream;
        ContentType = contentType;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileModel" /> class with the specified parameters.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="preSignedUrl">The pre-signed URL for the file.</param>
    /// <param name="expirationHours">The expiration time for the URL.</param>
    public FileModel(
        string bucketName,
        string fileName,
        string preSignedUrl,
        DateTime expirationHours) : base(bucketName)
    {
        Key = fileName;
        FileName = fileName;
        PreSignedUrl = preSignedUrl;
        UrlExpiration = expirationHours;
    }

    /// <summary>
    ///     Gets the key of the file in the bucket.
    /// </summary>
    public string? Key { get; private set; }

    /// <summary>
    ///     Gets the name of the file.
    /// </summary>
    public string? FileName { get; private set; }

    /// <summary>
    ///     Gets the file stream.
    /// </summary>
    public Stream? Stream { get; private set; }

    /// <summary>
    ///     Gets the content type of the file.
    /// </summary>
    public string? ContentType { get; private set; }

    /// <summary>
    ///     Gets the pre-signed URL for the file.
    /// </summary>
    public string? PreSignedUrl { get; private set; }

    /// <summary>
    ///     Gets the expiration time for the URL.
    /// </summary>
    public DateTime? UrlExpiration { get; private set; }

    /// <summary>
    ///     Sets the file stream.
    /// </summary>
    /// <param name="stream">The file stream.</param>
    public void SetStream(Stream stream)
    {
        Stream = stream;
    }

    /// <summary>
    ///     Sets the content type of the file.
    /// </summary>
    /// <param name="contentType">The content type of the file.</param>
    public void SetContentType(string contentType)
    {
        ContentType = contentType;
    }
}