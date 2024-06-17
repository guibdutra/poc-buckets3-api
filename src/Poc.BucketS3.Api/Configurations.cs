using System.Diagnostics.CodeAnalysis;
using Amazon.S3;
using Amazon.S3.Transfer;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Infrastructure.Services;

namespace Poc.BucketS3.Api;

/// <summary>
///     Static class for configuring dependency injection and AWS S3 services.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Configurations
{
    /// <summary>
    ///     Adds the necessary services for dependency injection.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IStorage, BucketS3Service>();
    }

    /// <summary>
    ///     Configures the AWS S3 services based on the environment (Debug/Release).
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    public static void AddAwsBucketS3(this IServiceCollection services)
    {
#if DEBUG
        services.AddScoped((Func<IServiceProvider, IAmazonS3>)(_ =>
            new AmazonS3Client(new AmazonS3Config
            {
                ServiceURL = "https://localhost.localstack.cloud:4566",
                ForcePathStyle = true
            })));
#else
        services.AddScoped((Func<IServiceProvider, IAmazonS3>)(_ =>
            new AmazonS3Client(Constants.Aws.UserKey, Constants.Aws.UserSecret, new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(Constants.Aws.AmazonS3.Region)
            })));
#endif
        services.AddScoped<ITransferUtility, TransferUtility>(delegate(IServiceProvider provider)
        {
            var requiredService = provider.GetRequiredService<IAmazonS3>();
            return new TransferUtility(requiredService);
        });
    }
}