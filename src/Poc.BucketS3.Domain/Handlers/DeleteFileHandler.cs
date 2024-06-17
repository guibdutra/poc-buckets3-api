using MediatR;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Handlers;

/// <summary>
///     Handler for deleting a file from an S3 bucket.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="DeleteFileHandler" /> class.
/// </remarks>
/// <param name="storage">The storage service for interacting with S3.</param>
/// <exception cref="ArgumentNullException">Thrown if the storage service is null.</exception>
public class DeleteFileHandler(IStorage storage) : IRequestHandler<DeleteFileCommand>
{
    private readonly IStorage _storage = storage ?? throw new ArgumentNullException(nameof(storage));

    /// <summary>
    ///     Handles the command to delete a file from an S3 bucket.
    /// </summary>
    /// <param name="request">The command request containing the bucket name and file key.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        await _storage.DeleteFileAsync(new FileModel(request.BucketName!, request.Key!), cancellationToken);
    }
}