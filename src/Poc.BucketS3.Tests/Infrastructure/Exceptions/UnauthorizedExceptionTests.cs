using System.Net;
using Microsoft.Extensions.Logging;
using Poc.BucketS3.Infrastructure.Exceptions;
using Xunit;

namespace Poc.BucketS3.Tests.Infrastructure.Exceptions;

public class UnauthorizedExceptionTests
{
    [Fact]
    public void UnauthorizedException_ShouldInitializeWithCustomMessage()
    {
        // Arrange
        const string message = "Custom unauthorized error message";

        // Act
        var exception = new UnauthorizedException(message);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        Assert.Equal(message, exception.Message);
        Assert.Equal(LogLevel.Information, exception.LogLevel);
    }

    [Fact]
    public void UnauthorizedException_ShouldInitializeWithDefaultMessage()
    {
        // Act
        var exception = new UnauthorizedException();

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        Assert.Equal("Unauthorized access. Please check your credentials and try again.", exception.Message);
        Assert.Equal(LogLevel.Information, exception.LogLevel);
    }
}