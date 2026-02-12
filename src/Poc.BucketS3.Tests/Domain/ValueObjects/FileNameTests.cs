using Poc.BucketS3.Domain.Exceptions;
using Poc.BucketS3.Domain.ValueObjects;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.ValueObjects;

public class FileNameTests
{
    [Fact]
    public void Constructor_ShouldCreateValidFileName_WhenValueIsValid()
    {
        const string validName = "document.pdf";
        var fileName = new FileName(validName);
        Assert.Equal(validName, fileName.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowDomainException_WhenNameIsEmptyOrWhitespace(string invalidName)
    {
        var exception = Assert.Throws<DomainException>(() => new FileName(invalidName));
        Assert.Contains("cannot be empty", exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowDomainException_WhenNameExceedsMaxLength()
    {
        var longName = new string('a', 1025);
        var exception = Assert.Throws<DomainException>(() => new FileName(longName));
        Assert.Contains("cannot exceed 1024 characters", exception.Message);
    }
}
