using MediatR;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Interfaces;

namespace Poc.BucketS3.Domain.Handlers;

/// <summary>
///     Handler for creating a new S3 bucket.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="CreateBucketHandler" /> class.
/// </remarks>
/// <param name="storage">The storage service for interacting with S3.</param>
/// <exception cref="ArgumentNullException">Thrown if the storage service is null.</exception>
public class CreateBucketHandler(IStorage storage) : IRequestHandler<CreateBucketCommand>
{
    private readonly IStorage _storage = storage ?? throw new ArgumentNullException(nameof(storage));

    /// <summary>
    ///     Handles the command to create a new S3 bucket.
    /// </summary>
    /// <param name="request">The command request containing the bucket name.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(CreateBucketCommand request, CancellationToken cancellationToken)
    {
        await _storage.CreateBucketAsync(request.BucketName!, cancellationToken);
    }
}