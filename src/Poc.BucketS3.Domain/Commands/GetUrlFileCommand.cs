using MediatR;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Commands;

/// <summary>
///     Command for retrieving a pre-signed URL for a file in an S3 bucket.
/// </summary>
/// <param name="bucketName">The name of the bucket containing the file.</param>
/// <param name="key">The key of the file to get the URL for.</param>
public class GetUrlFileCommand(string bucketName, string key) : BaseCommand(bucketName, key), IRequest<FileModel>
{
}