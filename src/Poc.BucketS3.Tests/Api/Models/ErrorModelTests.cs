using Poc.BucketS3.Api.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Api.Models;

public class ErrorModelTests
{
    [Fact]
    public void ErrorModel_ShouldInitializeCorrectly()
    {
        // Arrange
        const int statusCode = 404;
        const string errorMessage = "Resource not found";

        // Act
        var errorModel = new ErrorModel(statusCode, errorMessage);

        // Assert
        Assert.Equal(statusCode, errorModel.StatusCode);
        Assert.Equal(errorMessage, errorModel.Message);
    }

    [Fact]
    public void ErrorModel_ShouldHandleNullMessage()
    {
        // Arrange
        const int statusCode = 500;

        // Act
        var errorModel = new ErrorModel(statusCode, null);

        // Assert
        Assert.Equal(statusCode, errorModel.StatusCode);
        Assert.Null(errorModel.Message);
    }

    [Fact]
    public void ErrorModel_ShouldAllowPropertyModification()
    {
        // Arrange
        var errorModel = new ErrorModel(400, "Bad Request")
        {
            // Act
            StatusCode = 401,
            Message = "Unauthorized"
        };

        // Assert
        Assert.Equal(401, errorModel.StatusCode);
        Assert.Equal("Unauthorized", errorModel.Message);
    }
}