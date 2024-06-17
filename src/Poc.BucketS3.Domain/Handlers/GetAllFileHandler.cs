using MediatR;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Handlers;

/// <summary>
///     Handler for retrieving all files from an S3 bucket with optional filtering and pagination.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="GetAllFileHandler" /> class.
/// </remarks>
/// <param name="storage">The storage service for interacting with S3.</param>
/// <exception cref="ArgumentNullException">Thrown if the storage service is null.</exception>
public class GetAllFileHandler(IStorage storage) : IRequestHandler<GetAllFileCommand, (List<FileModel>, string)>
{
    private readonly IStorage _storage = storage ?? throw new ArgumentNullException(nameof(storage));

    /// <summary>
    ///     Handles the command to retrieve all files from an S3 bucket.
    /// </summary>
    /// <param name="request">The command request containing the bucket name, max keys, prefix, and continuation token.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing the list of file models and the continuation
    ///     token.
    /// </returns>
    public async Task<(List<FileModel>, string)> Handle(GetAllFileCommand request, CancellationToken cancellationToken)
    {
        var (lstFileModel, continuationToken) =
            await _storage.ListFilesAsync(
                new FileModel(request.BucketName!), request.MaxKeys, cancellationToken, request.ContinuationToken,
                request.Prefix);

        return (lstFileModel, continuationToken);
    }
}