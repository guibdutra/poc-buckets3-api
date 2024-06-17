using MediatR;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Commands;

/// <summary>
///     Command for uploading multiple files to an S3 bucket.
/// </summary>
/// <param name="listFile">The list of files to be uploaded.</param>
public class UploadFilesCommand(IEnumerable<FileModel> listFile) : IRequest
{
    /// <summary>
    ///     Gets the list of files to be uploaded.
    /// </summary>
    public IEnumerable<FileModel> ListFile { get; } = listFile;
}