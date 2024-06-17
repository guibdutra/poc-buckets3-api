using System.Net;
using Microsoft.Extensions.Logging;

namespace Poc.BucketS3.Infrastructure.Exceptions;

/// <summary>
///     Exception representing a conflict error (HTTP 409).
/// </summary>
public class ConflictException : BaseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConflictException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ConflictException(string message) : base(HttpStatusCode.Conflict, message, LogLevel.Information)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConflictException" /> class with a default error message.
    /// </summary>
    public ConflictException() : base(HttpStatusCode.Conflict,
        "A conflict has occurred. Please check your data and try again.",
        LogLevel.Information)
    {
    }
}