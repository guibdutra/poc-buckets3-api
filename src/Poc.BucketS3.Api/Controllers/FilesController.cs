using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Api.Controllers;

/// <summary>
///     Controller responsible for file operations in S3.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly ILogger<FilesController> _logger;
    private readonly IMediator _mediator;

    /// <summary>
    ///     Files controller constructor.
    /// </summary>
    /// <param name="logger">Logger instance for logging.</param>
    /// <param name="mediator">Mediator instance for mediating commands and queries.</param>
    /// <exception cref="ArgumentNullException">Thrown if logger or mediator are null.</exception>
    public FilesController(ILogger<FilesController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    ///     Uploads files to a specific bucket.
    /// </summary>
    /// <param name="bucketName">Name of the bucket where files will be uploaded.</param>
    /// <param name="files">List of files to be uploaded.</param>
    /// <param name="correlationId">Correlation ID for request tracking.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>Returns NoContent if successful, otherwise returns appropriate error codes.</returns>
    [HttpPost("upload")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> UploadFilesAsync(
        [FromQuery] [Required] string bucketName,
        [FromForm] [Required] IList<IFormFile> files,
        [FromHeader(Name = "X-Correlation-Id")]
        Guid? correlationId,
        CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Upload Files endpoint was called.");

        if (files.Count <= 0) throw new ArgumentNullException(nameof(files));

        List<FileModel> fileList =
        [
            .. files.Select(file =>
                new FileModel(bucketName, file.FileName, file.FileName, file.OpenReadStream(), file.ContentType))
        ];

        await _mediator.Send(new UploadFilesCommand(fileList), cancellationToken);
        return NoContent();
    }

    /// <summary>
    ///     Downloads a file from a specific bucket.
    /// </summary>
    /// <param name="bucketName">Name of the bucket from where the file will be downloaded.</param>
    /// <param name="key">Key of the file to be downloaded.</param>
    /// <param name="correlationId">Correlation ID for request tracking.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>Returns the file if successful, otherwise returns appropriate error codes.</returns>
    [HttpGet("download")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> GetFileAsync(
        [FromHeader] [Required] string bucketName,
        [FromQuery] [Required] string key,
        [FromHeader(Name = "X-Correlation-Id")]
        Guid? correlationId,
        CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Get File endpoint was called.");

        var result = await _mediator.Send(new DownloadFileCommand(bucketName, key), cancellationToken);

        AddResponseHeaders("Content-Disposition", $"attachment; filename={result.FileName}");
        return File(result.Stream!, result.ContentType!, result.FileName);
    }

    /// <summary>
    ///     Retrieves all files from a specific bucket.
    /// </summary>
    /// <param name="bucketName">Name of the bucket from where the files will be retrieved.</param>
    /// <param name="prefix">Optional prefix to filter the files.</param>
    /// <param name="correlationId">Correlation ID for request tracking.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <param name="continuationToken">Optional continuation token for pagination.</param>
    /// <param name="maxKeys">Maximum number of files to retrieve.</param>
    /// <returns>Returns the list of files if successful, otherwise returns appropriate error codes.</returns>
    [HttpGet("get-all")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> GetAllFileAsync(
        [FromQuery] [Required] string bucketName,
        [FromQuery] string? prefix,
        [FromHeader(Name = "X-Correlation-Id")]
        Guid? correlationId,
        CancellationToken cancellationToken,
        [FromHeader(Name = "X-Continuation-Token")]
        string? continuationToken = default,
        [FromQuery] int maxKeys = 100)
    {
        _logger.Log(LogLevel.Information, "Get All File endpoint was called.");

        var (lstFileModel, xContinuationToken) =
            await _mediator.Send(new GetAllFileCommand(bucketName, maxKeys, prefix, continuationToken),
                cancellationToken);

        AddResponseHeaders("X-Continuation-Token", xContinuationToken);
        return Ok(lstFileModel);
    }

    /// <summary>
    ///     Deletes a specific file from a bucket.
    /// </summary>
    /// <param name="bucketName">Name of the bucket from where the file will be deleted.</param>
    /// <param name="key">Key of the file to be deleted.</param>
    /// <param name="correlationId">Correlation ID for request tracking.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>Returns NoContent if successful, otherwise returns appropriate error codes.</returns>
    [HttpDelete("delete")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> DeleteFileAsync(
        [FromQuery] [Required] string bucketName,
        [FromQuery] [Required] string key,
        [FromHeader(Name = "X-Correlation-Id")]
        Guid? correlationId,
        CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Delete File endpoint was called.");

        await _mediator.Send(new DeleteFileCommand(bucketName, key), cancellationToken);
        return NoContent();
    }

    /// <summary>
    ///     Retrieves a pre-signed URL for a specific file in a bucket.
    /// </summary>
    /// <param name="bucketName">Name of the bucket where the file is located.</param>
    /// <param name="key">Key of the file to get the URL for.</param>
    /// <param name="correlationId">Correlation ID for request tracking.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>Returns the pre-signed URL if successful, otherwise returns appropriate error codes.</returns>
    [HttpGet("get-url")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> GetUrlFileAsync(
        [FromQuery] [Required] string bucketName,
        [FromQuery] [Required] string key,
        [FromHeader(Name = "X-Correlation-Id")]
        Guid? correlationId,
        CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information, "Get Url File endpoint was called.");

        var result = await _mediator.Send(new GetUrlFileCommand(bucketName, key), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    ///     Adds custom headers to the response.
    /// </summary>
    /// <param name="key">Header key.</param>
    /// <param name="value">Header value.</param>
    private void AddResponseHeaders(string key, string? value)
    {
        Response.Headers.Append(key, value);
    }
}