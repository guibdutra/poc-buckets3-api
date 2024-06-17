using System.Diagnostics.CodeAnalysis;
using System.Net;
using Poc.BucketS3.Api.Models;
using Poc.BucketS3.Infrastructure.Exceptions;

namespace Poc.BucketS3.Api.Middlewares;

/// <summary>
///     Middleware for handling exceptions globally in the application.
/// </summary>
[ExcludeFromCodeCoverage]
public class ExceptionMiddleware
{
    private const string DefaultExceptionMessage = "An error has occurred. Please try again later.";
    private const string CanceledExceptionMessage = "The operation was canceled. Please try again if necessary.";
    private const string DebugMessageTemplate = "An error was encountered. Details: {exception}.";
    private const string JsonContentType = "application/json";

    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Constructor for the ExceptionMiddleware.
    /// </summary>
    /// <param name="logger">Logger instance for logging exceptions.</param>
    /// <param name="next">The next middleware in the request pipeline.</param>
    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    /// <summary>
    ///     Middleware invocation method.
    /// </summary>
    /// <param name="context">HTTP context for the current request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    /// <summary>
    ///     Handles exceptions and constructs an appropriate HTTP response.
    /// </summary>
    /// <param name="context">HTTP context for the current request.</param>
    /// <param name="exception">The exception that was thrown.</param>
    /// <returns>A task that represents the completion of exception handling.</returns>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Determine the status code, message, and log level based on the exception type.
        var (statusCode, message, logLevel) = exception switch
        {
            ArgumentNullException or OperationCanceledException => (HttpStatusCode.BadRequest, CanceledExceptionMessage,
                LogLevel.Information),
            BadRequestException ex => (ex.StatusCode, ex.Message, ex.LogLevel),
            ConflictException ex => (ex.StatusCode, ex.Message, ex.LogLevel),
            NotFoundException ex => (ex.StatusCode, ex.Message, ex.LogLevel),
            UnauthorizedException ex => (ex.StatusCode, ex.Message, ex.LogLevel),
            _ => (HttpStatusCode.InternalServerError, DefaultExceptionMessage, LogLevel.Error)
        };

        // Log the exception with the appropriate log level.
        _logger.Log(logLevel, exception, DebugMessageTemplate, nameof(ExceptionMiddleware));

        // Set the response status code and content type.
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = JsonContentType;

        // Create an error response model and write it to the response.
        var errorResponse = new ErrorModel(context.Response.StatusCode, message);
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}