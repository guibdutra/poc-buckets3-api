using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Models;
using Poc.BucketS3.Infrastructure.Exceptions;

namespace Poc.BucketS3.Api.Controllers;

/// <summary>
///     Controller responsible for bucket operations in S3.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BucketsController : ControllerBase
{
    private readonly ILogger<BucketsController> _logger;
    private readonly IMediator _mediator;

    /// <summary>
    ///     Buckets controller constructor.
    /// </summary>
    /// <param name="logger">Logger instance for logging.</param>
    /// <param name="mediator">Mediator instance for mediating commands and queries.</param>
    /// <exception cref="ArgumentNullException">Thrown if logger or mediator are null.</exception>
    public BucketsController(ILogger<BucketsController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    ///     Creates new buckets.
    /// </summary>
    /// <param name="lstBucket">List of bucket models to be created.</param>
    /// <param name="correlationId">Correlation ID for request tracking.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>Returns NoContent if successful, otherwise returns appropriate error codes.</returns>
    [HttpPost("create")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> CreateBucketAsync(
        [FromBody] [Required] List<BucketModel> lstBucket,
        [FromHeader(Name = "X-Correlation-Id")]
        Guid? correlationId,
        CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Create Bucket endpoint was called.");

        if (lstBucket == null || lstBucket.Count == 0)
            throw new BadRequestException("The bucket list cannot be null or empty.");

        var tasks = lstBucket
            .Where(bucket => !string.IsNullOrWhiteSpace(bucket.BucketName))
            .Select(bucket => _mediator.Send(new CreateBucketCommand(bucket.BucketName), cancellationToken))
            .ToList();

        if (tasks.Count != lstBucket.Count)
            throw new BadRequestException("All buckets must have a valid name.");

        await Task.WhenAll(tasks);

        return NoContent();
    }

    /// <summary>
    ///     Retrieves all buckets.
    /// </summary>
    /// <param name="correlationId">Correlation ID for request tracking.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>Returns the list of buckets if successful, otherwise returns appropriate error codes.</returns>
    [HttpGet("get-all")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> GetAllBucketAsync(
        [FromHeader(Name = "X-Correlation-Id")]
        Guid? correlationId,
        CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Get All Bucket endpoint was called.");

        var result = await _mediator.Send(new GetAllBucketCommand(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    ///     Deletes a specific bucket.
    /// </summary>
    /// <param name="bucketName">Name of the bucket to be deleted.</param>
    /// <param name="correlationId">Correlation ID for request tracking.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>Returns NoContent if successful, otherwise returns appropriate error codes.</returns>
    [HttpDelete("delete")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> DeleteBucketAsync(
        [FromQuery] [Required] string bucketName,
        [FromHeader(Name = "X-Correlation-Id")]
        Guid? correlationId,
        CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Delete Bucket endpoint was called.");

        await _mediator.Send(new DeleteBucketCommand(bucketName), cancellationToken);
        return NoContent();
    }
}