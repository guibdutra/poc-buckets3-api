using MediatR;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Interfaces;

namespace Poc.BucketS3.Domain.Handlers;

/// <summary>
///     Handler for deleting an S3 bucket.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="DeleteBucketHandler" /> class.
/// </remarks>
/// <param name="storage">The storage service for interacting with S3.</param>
/// <exception cref="ArgumentNullException">Thrown if the storage service is null.</exception>
public class DeleteBucketHandler(IStorage storage) : IRequestHandler<DeleteBucketCommand>
{
    private readonly IStorage _storage = storage ?? throw new ArgumentNullException(nameof(storage));

    /// <summary>
    ///     Handles the command to delete an S3 bucket.
    /// </summary>
    /// <param name="request">The command request containing the bucket name.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(DeleteBucketCommand request, CancellationToken cancellationToken)
    {
        await _storage.DeleteBucketAsync(request.BucketName!, cancellationToken);
    }
}