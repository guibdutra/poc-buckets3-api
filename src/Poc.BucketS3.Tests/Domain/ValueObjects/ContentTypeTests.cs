using Poc.BucketS3.Domain.ValueObjects;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.ValueObjects;

public class ContentTypeTests
{
    [Fact]
    public void Constructor_ShouldUseDefaultValue_WhenValueIsNull()
    {
        var contentType = new ContentType(null);
        Assert.Equal("application/octet-stream", contentType.Value);
        Assert.True(contentType.IsDefault);
    }

    [Fact]
    public void Constructor_ShouldUseProvidedValue_WhenValid()
    {
        const string mimeType = "application/json";
        var contentType = new ContentType(mimeType);
        Assert.Equal(mimeType, contentType.Value);
        Assert.False(contentType.IsDefault);
    }
}
