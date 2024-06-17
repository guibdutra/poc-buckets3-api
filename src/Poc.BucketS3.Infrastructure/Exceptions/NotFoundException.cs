using System.Net;
using Microsoft.Extensions.Logging;

namespace Poc.BucketS3.Infrastructure.Exceptions;

/// <summary>
///     Exception representing a not found error (HTTP 404).
/// </summary>
public class NotFoundException : BaseException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="NotFoundException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public NotFoundException(string message) : base(HttpStatusCode.NotFound, message, LogLevel.Information)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotFoundException" /> class with a default error message.
    /// </summary>
    public NotFoundException() : base(HttpStatusCode.NotFound,
        "The requested resource could not be found. Please verify the information and try again.",
        LogLevel.Information)
    {
    }
}