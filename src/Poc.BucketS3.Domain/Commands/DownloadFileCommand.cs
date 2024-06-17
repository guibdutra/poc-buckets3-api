using MediatR;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Commands;

/// <summary>
///     Command for downloading a file from an S3 bucket.
/// </summary>
/// <param name="bucketName">The name of the bucket containing the file to be downloaded.</param>
/// <param name="key">The key of the file to be downloaded.</param>
public class DownloadFileCommand(string bucketName, string key) : BaseCommand(bucketName, key), IRequest<FileModel>
{
}