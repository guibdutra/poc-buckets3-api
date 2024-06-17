using MediatR;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Handlers;

/// <summary>
///     Handler for downloading a file from an S3 bucket.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="DownloadFileHandler" /> class.
/// </remarks>
/// <param name="storage">The storage service for interacting with S3.</param>
/// <exception cref="ArgumentNullException">Thrown if the storage service is null.</exception>
public class DownloadFileHandler(IStorage storage) : IRequestHandler<DownloadFileCommand, FileModel>
{
    private readonly IStorage _storage = storage ?? throw new ArgumentNullException(nameof(storage));

    /// <summary>
    ///     Handles the command to download a file from an S3 bucket.
    /// </summary>
    /// <param name="request">The command request containing the bucket name and file key.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation, containing the downloaded file model.</returns>
    public async Task<FileModel> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
    {
        return await _storage.GetFileAsync(new FileModel(request.BucketName!, request.Key!), cancellationToken);
    }
}