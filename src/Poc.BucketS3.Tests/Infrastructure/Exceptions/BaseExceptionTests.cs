using System.Net;
using Microsoft.Extensions.Logging;
using Poc.BucketS3.Infrastructure.Exceptions;
using Xunit;

namespace Poc.BucketS3.Tests.Infrastructure.Exceptions;

public class BaseExceptionTests
{
    [Fact]
    public void BaseException_ShouldInitializeCorrectly()
    {
        // Arrange
        const HttpStatusCode statusCode = HttpStatusCode.NotFound;
        const string message = "Resource not found";
        const LogLevel logLevel = LogLevel.Warning;

        // Act
        var exception = new TestBaseException(statusCode, message, logLevel);

        // Assert
        Assert.Equal(statusCode, exception.StatusCode);
        Assert.Equal(message, exception.Message);
        Assert.Equal(logLevel, exception.LogLevel);
    }

    [Fact]
    public void BaseException_ShouldInitializeWithDefaultLogLevel()
    {
        // Arrange
        const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        const string message = "An internal error occurred";

        // Act
        var exception = new TestBaseException(statusCode, message);

        // Assert
        Assert.Equal(statusCode, exception.StatusCode);
        Assert.Equal(message, exception.Message);
        Assert.Equal(LogLevel.Error, exception.LogLevel);
    }

    private class TestBaseException(HttpStatusCode statusCode, string message, LogLevel logLevel = LogLevel.Error)
        : BaseException(statusCode, message, logLevel);
}