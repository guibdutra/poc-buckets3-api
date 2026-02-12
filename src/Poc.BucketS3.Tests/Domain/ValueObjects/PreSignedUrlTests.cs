using Poc.BucketS3.Domain.Exceptions;
using Poc.BucketS3.Domain.ValueObjects;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.ValueObjects;

public class PreSignedUrlTests
{
    [Fact]
    public void Constructor_ShouldCreateValidUrl_WhenParametersAreValid()
    {
        const string url = "https://s3.amazonaws.com/bucket/file.txt?signature=abc123";
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var preSignedUrl = new PreSignedUrl(url, expiresAt);
        
        Assert.Equal(url, preSignedUrl.Url);
        Assert.False(preSignedUrl.IsExpired);
    }

    [Fact]
    public void Constructor_ShouldThrowDomainException_WhenExpirationIsInPast()
    {
        const string url = "https://s3.amazonaws.com/bucket/file.txt";
        var expiresAt = DateTime.UtcNow.AddHours(-1);
        
        var exception = Assert.Throws<DomainException>(() => new PreSignedUrl(url, expiresAt));
        Assert.Contains("must be in the future", exception.Message);
    }
}
