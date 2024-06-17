using MediatR;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Handlers;

/// <summary>
///     Handler for retrieving all S3 buckets.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="GetAllBucketHandler" /> class.
/// </remarks>
/// <param name="storage">The storage service for interacting with S3.</param>
/// <exception cref="ArgumentNullException">Thrown if the storage service is null.</exception>
public class GetAllBucketHandler(IStorage storage) : IRequestHandler<GetAllBucketCommand, List<BucketModel>>
{
    private readonly IStorage _storage = storage ?? throw new ArgumentNullException(nameof(storage));

    /// <summary>
    ///     Handles the command to retrieve all S3 buckets.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation, containing the list of bucket models.</returns>
    public async Task<List<BucketModel>> Handle(GetAllBucketCommand request, CancellationToken cancellationToken)
    {
        return await _storage.ListBucketsAsync(cancellationToken);
    }
}