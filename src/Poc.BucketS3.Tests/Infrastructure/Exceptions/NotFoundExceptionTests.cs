using System.Net;
using Microsoft.Extensions.Logging;
using Poc.BucketS3.Infrastructure.Exceptions;
using Xunit;

namespace Poc.BucketS3.Tests.Infrastructure.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void NotFoundException_ShouldInitializeWithCustomMessage()
    {
        // Arrange
        const string message = "Custom not found error message";

        // Act
        var exception = new NotFoundException(message);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        Assert.Equal(message, exception.Message);
        Assert.Equal(LogLevel.Information, exception.LogLevel);
    }

    [Fact]
    public void NotFoundException_ShouldInitializeWithDefaultMessage()
    {
        // Act
        var exception = new NotFoundException();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        Assert.Equal("The requested resource could not be found. Please verify the information and try again.",
            exception.Message);
        Assert.Equal(LogLevel.Information, exception.LogLevel);
    }
}