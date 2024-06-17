using MediatR;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Interfaces;

namespace Poc.BucketS3.Domain.Handlers;

/// <summary>
///     Handler for uploading multiple files to an S3 bucket.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="UploadFilesHandler" /> class.
/// </remarks>
/// <param name="storage">The storage service for interacting with S3.</param>
/// <exception cref="ArgumentNullException">Thrown if the storage service is null.</exception>
public class UploadFilesHandler(IStorage storage) : IRequestHandler<UploadFilesCommand>
{
    private readonly IStorage _storage = storage ?? throw new ArgumentNullException(nameof(storage));

    /// <summary>
    ///     Handles the command to upload multiple files to an S3 bucket.
    /// </summary>
    /// <param name="request">The command request containing the list of files to be uploaded.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(UploadFilesCommand request, CancellationToken cancellationToken)
    {
        List<Task> uploadTasks =
            [.. request.ListFile.Select(file => _storage.UploadFileAsync(file, cancellationToken))];
        await Task.WhenAll(uploadTasks);
    }
}