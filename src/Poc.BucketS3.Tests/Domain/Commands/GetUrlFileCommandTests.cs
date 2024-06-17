using Poc.BucketS3.Domain.Commands;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Commands;

public class GetUrlFileCommandTests
{
    [Fact]
    public void GetUrlFileCommand_ShouldInitializeCorrectly()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string key = "test-key.pdf";

        // Act
        var command = new GetUrlFileCommand(bucketName, key);

        // Assert
        Assert.Equal(bucketName, command.BucketName);
        Assert.Equal(key, command.Key);
    }

    [Fact]
    public void GetUrlFileCommand_ShouldInheritBaseCommand()
    {
        // Arrange & Act
        var command = new GetUrlFileCommand("test-bucket", "test-key.pdf");

        // Assert
        Assert.IsAssignableFrom<BaseCommand>(command);
    }
}