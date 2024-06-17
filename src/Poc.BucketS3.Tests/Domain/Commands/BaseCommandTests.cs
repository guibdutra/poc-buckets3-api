using Poc.BucketS3.Domain.Commands;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Commands;

public class BaseCommandTests
{
    [Fact]
    public void BaseCommand_ShouldInitializeCorrectly()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string key = "test-key.pdf";

        // Act
        var command = new BaseCommand(bucketName, key);

        // Assert
        Assert.Equal(bucketName, command.BucketName);
        Assert.Equal(key, command.Key);
    }

    [Fact]
    public void BaseCommand_ShouldHandleDefaultValues()
    {
        // Act
        var command = new BaseCommand();

        // Assert
        Assert.Null(command.BucketName);
        Assert.Null(command.Key);
    }

    [Fact]
    public void BaseCommand_ShouldAllowPropertyModification()
    {
        // Arrange
        var command = new BaseCommand("initial-bucket", "initial-key.pdf")
        {
            // Act
            BucketName = "modified-bucket",
            Key = "modified-key.pdf"
        };

        // Assert
        Assert.Equal("modified-bucket", command.BucketName);
        Assert.Equal("modified-key.pdf", command.Key);
    }
}