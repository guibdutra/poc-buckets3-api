using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Models;

public class BaseModelTests
{
    [Fact]
    public void BaseModel_ShouldInitializeCorrectly()
    {
        // Arrange
        const string bucketName = "test-bucket";

        // Act
        var model = new BaseModel(bucketName);

        // Assert
        Assert.Equal(bucketName, model.BucketName);
    }

    [Fact]
    public void BaseModel_BucketName_ShouldBeReadOnly()
    {
        // Arrange
        const string bucketName = "test-bucket";

        // Act
        var model = new BaseModel(bucketName);

        // Assert
        var type = model.GetType();
        var property = type.GetProperty("BucketName");
        Assert.True(property!.CanRead);
        Assert.False(property.CanWrite);
    }
}