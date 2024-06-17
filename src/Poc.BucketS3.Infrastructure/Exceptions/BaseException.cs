using System.Net;
using Microsoft.Extensions.Logging;

namespace Poc.BucketS3.Infrastructure.Exceptions;

/// <summary>
///     Base exception class for custom exceptions in the application.
/// </summary>
public class BaseException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseException" /> class with a specified status code, error message,
    ///     and log level.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="logLevel">
    ///     The log level for logging the exception. Defaults to
    ///     <see cref="Microsoft.Extensions.Logging.LogLevel.Error" />.
    /// </param>
    protected BaseException(HttpStatusCode statusCode, string message, LogLevel logLevel = LogLevel.Error) :
        base(message)
    {
        StatusCode = statusCode;
        LogLevel = logLevel;
    }

    /// <summary>
    ///     Gets or sets the HTTP status code associated with the exception.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    ///     Gets or sets the log level for logging the exception.
    /// </summary>
    public LogLevel LogLevel { get; set; }
}