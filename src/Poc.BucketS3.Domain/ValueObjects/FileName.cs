using Poc.BucketS3.Domain.Exceptions;

namespace Poc.BucketS3.Domain.ValueObjects;

/// <summary>
///     Value Object representing a valid file name/key in S3.
///     
///     SOLID Principles Applied:
///     - Single Responsibility: Only handles file name validation
///     
///     DDD Pattern: Value Object
///     - Immutable
///     - Self-validating
/// </summary>
public record FileName
{
    private const int MaxLength = 1024; // S3 limit

    public string Value { get; }

    public FileName(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var trimmedValue = value.Trim();

        if (string.IsNullOrWhiteSpace(trimmedValue))
            throw new DomainException("File name cannot be empty or whitespace.");

        if (trimmedValue.Length > MaxLength)
            throw new DomainException($"File name cannot exceed {MaxLength} characters. Provided: {trimmedValue.Length}");

        Value = trimmedValue;
    }

    public override string ToString() => Value;

    public static implicit operator string(FileName fileName) => fileName.Value;
}
