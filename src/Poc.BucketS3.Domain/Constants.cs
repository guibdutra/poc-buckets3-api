using System.Diagnostics.CodeAnalysis;

namespace Poc.BucketS3.Domain;

/// <summary>
///     Static class containing application constants and environment variables.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Constants
{
    /// <summary>
    ///     Gets the environment name.
    /// </summary>
    public static string Environment => GetEnvironmentVariable("ENVIRONMENT").ToLower();

    /// <summary>
    ///     Retrieves the value of an environment variable.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <returns>The value of the environment variable.</returns>
    /// <exception cref="ApplicationException">Thrown if the environment variable is not found.</exception>
    private static string GetEnvironmentVariable(string variable)
    {
        return System.Environment.GetEnvironmentVariable(variable) ??
               throw new ApplicationException($"Environment {variable} not informed.");
    }

    /// <summary>
    ///     Static class containing AWS-related constants and environment variables.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Aws
    {
        /// <summary>
        ///     Gets the AWS user key from environment variables.
        /// </summary>
        public static readonly string UserKey = GetEnvironmentVariable("AWS_USER_KEY");

        /// <summary>
        ///     Gets the AWS user secret from environment variables.
        /// </summary>
        public static readonly string UserSecret = GetEnvironmentVariable("AWS_USER_SECRET");

        /// <summary>
        ///     Static class containing S3 bucket-related constants.
        /// </summary>
        public static class BucketS3
        {
            /// <summary>
            ///     Gets the AWS region for S3 buckets from environment variables.
            /// </summary>
            public static readonly string Region = GetEnvironmentVariable("AWS_REGION");
        }
    }
}