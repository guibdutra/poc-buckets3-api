using Poc.BucketS3.Domain.Commands;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Commands;

public class CreateBucketCommandTests
{
    [Fact]
    public void CreateBucketCommand_ShouldInitializeCorrectly()
    {
        // Arrange
        const string bucketName = "test-bucket";

        // Act
        var command = new CreateBucketCommand(bucketName);

        // Assert
        Assert.Equal(bucketName, command.BucketName);
        Assert.Null(command.Key); // Ensure the base class property 'Key' is null
    }

    [Fact]
    public void CreateBucketCommand_ShouldInheritBaseCommand()
    {
        // Arrange & Act
        var command = new CreateBucketCommand("test-bucket");

        // Assert
        Assert.IsAssignableFrom<BaseCommand>(command);
    }
}