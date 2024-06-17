using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Models;

public class BucketModelTests
{
    [Fact]
    public void BucketModel_ShouldInitializeCorrectly()
    {
        // Arrange
        const string bucketName = "test-bucket";

        // Act
        var model = new BucketModel(bucketName);

        // Assert
        Assert.Equal(bucketName, model.BucketName);
    }

    [Fact]
    public void BucketModel_ShouldInheritBaseModel()
    {
        // Arrange
        const string bucketName = "test-bucket";

        // Act
        var model = new BucketModel(bucketName);

        // Assert
        Assert.IsAssignableFrom<BaseModel>(model);
    }
}