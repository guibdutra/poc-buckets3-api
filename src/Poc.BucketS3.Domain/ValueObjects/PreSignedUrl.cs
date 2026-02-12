using Poc.BucketS3.Domain.Exceptions;

namespace Poc.BucketS3.Domain.ValueObjects;

/// <summary>
///     Value Object representing a pre-signed URL for temporary file access.
///     
///     DDD Pattern: Value Object
///     - Immutable
///     - Encapsulates URL and expiration
/// </summary>
public record PreSignedUrl
{
    public string Url { get; }
    public DateTime ExpiresAt { get; }

    public PreSignedUrl(string url, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("Pre-signed URL cannot be empty.");

        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            throw new DomainException($"Invalid URL format: {url}");

        if (expiresAt <= DateTime.UtcNow)
            throw new DomainException($"Pre-signed URL expiration must be in the future. Provided: {expiresAt}");

        Url = url;
        ExpiresAt = expiresAt;
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public TimeSpan TimeUntilExpiration => ExpiresAt - DateTime.UtcNow;

    public override string ToString() => Url;

    public static implicit operator string(PreSignedUrl preSignedUrl) => preSignedUrl.Url;
}
