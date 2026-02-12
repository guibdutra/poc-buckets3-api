using Poc.BucketS3.Domain.Exceptions;

namespace Poc.BucketS3.Domain.ValueObjects;

/// <summary>
///     Value Object representing a valid S3 bucket name.
///     Encapsulates bucket name validation rules following AWS S3 naming conventions.
///     
///     SOLID Principles Applied:
///     - Single Responsibility: Only handles bucket name validation and formatting
///     - Open/Closed: Closed for modification, open for extension via inheritance
///     
///     DDD Pattern: Value Object
///     - Immutable
///     - Self-validating
///     - Equality based on value, not identity
/// </summary>
public record BucketName
{
    private const int MinLength = 3;
    private const int MaxLength = 63;

    public string Value { get; }

    public BucketName(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var trimmedValue = value.Trim().ToLowerInvariant();

        if (trimmedValue.Length < MinLength || trimmedValue.Length > MaxLength)
            throw new DomainException($"Bucket name must be between {MinLength} and {MaxLength} characters. Provided: {trimmedValue.Length}");

        if (!IsValidFormat(trimmedValue))
            throw new DomainException($"Bucket name '{trimmedValue}' contains invalid characters. Must contain only lowercase letters, numbers, hyphens, and periods.");

        Value = trimmedValue;
    }

    private static bool IsValidFormat(string name)
    {
        // Basic validation - can be extended
        return name.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '.');
    }

    public override string ToString() => Value;

    public static implicit operator string(BucketName bucketName) => bucketName.Value;
}
