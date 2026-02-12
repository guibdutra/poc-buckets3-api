using System.Diagnostics.CodeAnalysis;

namespace Poc.BucketS3.Domain.Exceptions;

/// <summary>
///     Base exception for domain-level business rule violations.
///     Follows DDD principle of keeping domain logic separate from infrastructure.
/// </summary>
[ExcludeFromCodeCoverage]
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
