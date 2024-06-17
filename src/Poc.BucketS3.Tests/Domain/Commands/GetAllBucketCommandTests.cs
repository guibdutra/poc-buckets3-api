using Poc.BucketS3.Domain.Commands;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Commands;

public class GetAllBucketCommandTests
{
    [Fact]
    public void GetAllBucketCommand_ShouldInitializeCorrectly()
    {
        // Act
        var command = new GetAllBucketCommand();

        // Assert
        Assert.IsType<GetAllBucketCommand>(command);
    }
}