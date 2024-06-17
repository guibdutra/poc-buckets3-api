using System.Diagnostics.CodeAnalysis;
using Serilog.Context;

namespace Poc.BucketS3.Api.Middlewares;

/// <summary>
///     Middleware for handling Correlation IDs in HTTP requests and responses.
/// </summary>
[ExcludeFromCodeCoverage]
public class CorrelationIdMiddleware
{
    private const string CorrelationId = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Constructor for the CorrelationIdMiddleware.
    /// </summary>
    /// <param name="next">The next middleware in the request pipeline.</param>
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///     Middleware invocation method.
    /// </summary>
    /// <param name="context">HTTP context for the current request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the Correlation ID is present in the request headers, if not, generate a new one
        if (!context.Request.Headers.TryGetValue(CorrelationId, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers[CorrelationId] = correlationId;
        }

        // Add the Correlation ID to the response headers
        context.Response.Headers[CorrelationId] = correlationId;

        // Push the Correlation ID into the Serilog context for logging
        using (LogContext.PushProperty(nameof(CorrelationId), correlationId))
        {
            await _next(context);
        }
    }
}