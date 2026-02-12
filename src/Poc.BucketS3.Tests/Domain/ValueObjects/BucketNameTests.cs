using Poc.BucketS3.Domain.Exceptions;
using Poc.BucketS3.Domain.ValueObjects;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.ValueObjects;

/// <summary>
///     Tests for BucketName value object.
///     Ensures validation rules are enforced per AWS S3 bucket naming conventions.
/// </summary>
public class BucketNameTests
{
    [Fact]
    public void Constructor_ShouldCreateValidBucketName_WhenValueIsValid()
    {
        // Arrange
        const string validName = "my-bucket-123";

        // Act
        var bucketName = new BucketName(validName);

        // Assert
        Assert.Equal(validName, bucketName.Value);
    }

    [Fact]
    public void Constructor_ShouldNormalizeName_ToLowerCase()
    {
        // Arrange
        const string mixedCase = "MyBucket";

        // Act
        var bucketName = new BucketName(mixedCase);

        // Assert
        Assert.Equal("mybucket", bucketName.Value);
    }

    [Theory]
    [InlineData("ab")]        // Too short (< 3 chars)
    [InlineData("a")]         // Way too short
    public void Constructor_ShouldThrowDomainException_WhenNameIsTooShort(string shortName)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new BucketName(shortName));
        Assert.Contains("must be between 3 and 63 characters", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowDomainException_WhenNameIsTooLong()
    {
        // Arrange
        var longName = new string('a', 64); // Exceeds 63 char limit

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new BucketName(longName));
        Assert.Contains("must be between 3 and 63 characters", exception.Message);
    }

    [Theory]
    [InlineData("my_bucket")]     // Underscores not allowed
    [InlineData("my bucket")]     // Spaces not allowed
    [InlineData("my@bucket")]     // Special chars not allowed
    public void Constructor_ShouldThrowDomainException_WhenNameContainsInvalidCharacters(string invalidName)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new BucketName(invalidName));
        Assert.Contains("invalid characters", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenValueIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BucketName(null!));
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string name = "test-bucket";
        var bucketName = new BucketName(name);

        // Act
        var result = bucketName.ToString();

        // Assert
        Assert.Equal(name, result);
    }

    [Fact]
    public void ImplicitOperator_ShouldConvertToString()
    {
        // Arrange
        var bucketName = new BucketName("test-bucket");

        // Act
        string result = bucketName;

        // Assert
        Assert.Equal("test-bucket", result);
    }

    [Fact]
    public void Equality_ShouldBeBasedOnValue()
    {
        // Arrange
        var bucket1 = new BucketName("my-bucket");
        var bucket2 = new BucketName("my-bucket");
        var bucket3 = new BucketName("other-bucket");

        // Assert
        Assert.Equal(bucket1, bucket2);
        Assert.NotEqual(bucket1, bucket3);
    }
}
