namespace Poc.BucketS3.Api.Models;

/// <summary>
///     Model for representing error details in HTTP responses.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ErrorModel" /> class.
/// </remarks>
/// <param name="statusCode">The HTTP status code associated with the error.</param>
/// <param name="message">The error message.</param>
public class ErrorModel(int statusCode, string? message)
{
    /// <summary>
    ///     Gets or sets the HTTP status code.
    /// </summary>
    public int StatusCode { get; set; } = statusCode;

    /// <summary>
    ///     Gets or sets the error message.
    /// </summary>
    public string? Message { get; set; } = message;
}