using System.Net;
using Microsoft.Extensions.Logging;
using Poc.BucketS3.Infrastructure.Exceptions;
using Xunit;

namespace Poc.BucketS3.Tests.Infrastructure.Exceptions;

public class ConflictExceptionTests
{
    [Fact]
    public void ConflictException_ShouldInitializeWithCustomMessage()
    {
        // Arrange
        const string message = "Custom conflict error message";

        // Act
        var exception = new ConflictException(message);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
        Assert.Equal(message, exception.Message);
        Assert.Equal(LogLevel.Information, exception.LogLevel);
    }

    [Fact]
    public void ConflictException_ShouldInitializeWithDefaultMessage()
    {
        // Act
        var exception = new ConflictException();

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
        Assert.Equal("A conflict has occurred. Please check your data and try again.", exception.Message);
        Assert.Equal(LogLevel.Information, exception.LogLevel);
    }
}