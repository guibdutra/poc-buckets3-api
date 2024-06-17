using System.Net;
using Microsoft.Extensions.Logging;
using Poc.BucketS3.Infrastructure.Exceptions;
using Xunit;

namespace Poc.BucketS3.Tests.Infrastructure.Exceptions;

public class BadRequestExceptionTests
{
    [Fact]
    public void BadRequestException_ShouldInitializeWithCustomMessage()
    {
        // Arrange
        const string message = "Custom error message";

        // Act
        var exception = new BadRequestException(message);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal(message, exception.Message);
        Assert.Equal(LogLevel.Information, exception.LogLevel);
    }

    [Fact]
    public void BadRequestException_ShouldInitializeWithDefaultMessage()
    {
        // Act
        var exception = new BadRequestException();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal("A bad request has occurred. Please check your data and try again.", exception.Message);
        Assert.Equal(LogLevel.Information, exception.LogLevel);
    }
}