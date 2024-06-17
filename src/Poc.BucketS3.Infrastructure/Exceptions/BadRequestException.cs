using System.Net;
using Microsoft.Extensions.Logging;

namespace Poc.BucketS3.Infrastructure.Exceptions;

/// <summary>
///     Exception representing a bad request error (HTTP 400).
/// </summary>
public class BadRequestException : BaseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BadRequestException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message, LogLevel.Information)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BadRequestException" /> class with a default error message.
    /// </summary>
    public BadRequestException() : base(HttpStatusCode.BadRequest,
        "A bad request has occurred. Please check your data and try again.",
        LogLevel.Information)
    {
    }
}