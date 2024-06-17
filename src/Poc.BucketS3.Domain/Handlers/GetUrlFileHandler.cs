using MediatR;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Handlers;

/// <summary>
///     Handler for retrieving a pre-signed URL for a file in an S3 bucket.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="GetUrlFileHandler" /> class.
/// </remarks>
/// <param name="storage">The storage service for interacting with S3.</param>
/// <exception cref="ArgumentNullException">Thrown if the storage service is null.</exception>
public class GetUrlFileHandler(IStorage storage) : IRequestHandler<GetUrlFileCommand, FileModel>
{
    private readonly IStorage _storage = storage ?? throw new ArgumentNullException(nameof(storage));

    /// <summary>
    ///     Handles the command to retrieve a pre-signed URL for a file in an S3 bucket.
    /// </summary>
    /// <param name="request">The command request containing the bucket name and file key.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation, containing the file model with the pre-signed URL.</returns>
    public async Task<FileModel> Handle(GetUrlFileCommand request, CancellationToken cancellationToken)
    {
        return await _storage.GetUrlFileAsync(new FileModel(request.BucketName!, request.Key!), cancellationToken);
    }
}