using System.Net;
using Microsoft.Extensions.Logging;

namespace Poc.BucketS3.Infrastructure.Exceptions;

/// <summary>
///     Exception representing an unauthorized access error (HTTP 401).
/// </summary>
public class UnauthorizedException : BaseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UnauthorizedException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public UnauthorizedException(string message) : base(HttpStatusCode.Unauthorized, message, LogLevel.Information)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="UnauthorizedException" /> class with a default error message.
    /// </summary>
    public UnauthorizedException() : base(HttpStatusCode.Unauthorized,
        "Unauthorized access. Please check your credentials and try again.",
        LogLevel.Information)
    {
    }
}