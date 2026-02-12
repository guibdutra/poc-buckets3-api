namespace Poc.BucketS3.Domain.ValueObjects;

/// <summary>
///     Value Object representing a file's content type (MIME type).
///     
///     DDD Pattern: Value Object
///     - Immutable
///     - Has default value for unknown types
/// </summary>
public record ContentType
{
    private const string DefaultValue = "application/octet-stream";

    public string Value { get; }

    public ContentType(string? value)
    {
        Value = string.IsNullOrWhiteSpace(value) ? DefaultValue : value.Trim();
    }

    public bool IsDefault => Value == DefaultValue;

    public override string ToString() => Value;

    public static implicit operator string(ContentType contentType) => contentType.Value;

    public static ContentType Default => new(DefaultValue);
}
