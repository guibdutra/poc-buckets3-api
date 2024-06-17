using Poc.BucketS3.Domain.Commands;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Commands;

public class GetAllFileCommandTests
{
    [Fact]
    public void GetAllFileCommand_ShouldInitializeCorrectly()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const int maxKeys = 10;
        const string prefix = "test-prefix";
        const string continuationToken = "test-token";

        // Act
        var command = new GetAllFileCommand(bucketName, maxKeys, prefix, continuationToken);

        // Assert
        Assert.Equal(bucketName, command.BucketName);
        Assert.Equal(maxKeys, command.MaxKeys);
        Assert.Equal(prefix, command.Prefix);
        Assert.Equal(continuationToken, command.ContinuationToken);
    }

    [Fact]
    public void GetAllFileCommand_ShouldInitializeCorrectlyWithoutOptionalParameters()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const int maxKeys = 10;

        // Act
        var command = new GetAllFileCommand(bucketName, maxKeys);

        // Assert
        Assert.Equal(bucketName, command.BucketName);
        Assert.Equal(maxKeys, command.MaxKeys);
        Assert.Null(command.Prefix);
        Assert.Null(command.ContinuationToken);
    }

    [Fact]
    public void GetAllFileCommand_ShouldInheritBaseCommand()
    {
        // Arrange & Act
        var command = new GetAllFileCommand("test-bucket", 10);

        // Assert
        Assert.IsAssignableFrom<BaseCommand>(command);
    }
}