using Poc.BucketS3.Domain.Commands;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Commands;

public class DeleteFileCommandTests
{
    [Fact]
    public void DeleteFileCommand_ShouldInitializeCorrectly()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string key = "test-key.pdf";

        // Act
        var command = new DeleteFileCommand(bucketName, key);

        // Assert
        Assert.Equal(bucketName, command.BucketName);
        Assert.Equal(key, command.Key);
    }

    [Fact]
    public void DeleteFileCommand_ShouldInheritBaseCommand()
    {
        // Arrange & Act
        var command = new DeleteFileCommand("test-bucket", "test-key.pdf");

        // Assert
        Assert.IsAssignableFrom<BaseCommand>(command);
    }
}