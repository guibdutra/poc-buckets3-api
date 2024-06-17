using Poc.BucketS3.Domain.Commands;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Commands;

public class DownloadFileCommandTests
{
    [Fact]
    public void DownloadFileCommand_ShouldInitializeCorrectly()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string key = "test-key.pdf";

        // Act
        var command = new DownloadFileCommand(bucketName, key);

        // Assert
        Assert.Equal(bucketName, command.BucketName);
        Assert.Equal(key, command.Key);
    }

    [Fact]
    public void DownloadFileCommand_ShouldInheritBaseCommand()
    {
        // Arrange & Act
        var command = new DownloadFileCommand("test-bucket", "test-key.pdf");

        // Assert
        Assert.IsAssignableFrom<BaseCommand>(command);
    }
}