using MediatR;
using Poc.BucketS3.Domain.Models;

namespace Poc.BucketS3.Domain.Commands;

/// <summary>
///     Command for retrieving all S3 buckets.
/// </summary>
public class GetAllBucketCommand : IRequest<List<BucketModel>>
{
}