using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Moq;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;
using Poc.BucketS3.Infrastructure.Exceptions;
using Poc.BucketS3.Infrastructure.Services;
using Xunit;

namespace Poc.BucketS3.Tests.Infrastructure.Services;

public class BucketS3ServiceTests
{
    private readonly Mock<IAmazonS3> _amazonS3Mock;
    private readonly IStorage _bucketS3Service;
    private readonly Mock<ITransferUtility> _transferUtilityMock;

    public BucketS3ServiceTests()
    {
        _amazonS3Mock = new Mock<IAmazonS3>();
        _transferUtilityMock = new Mock<ITransferUtility>();
        _bucketS3Service = new BucketS3Service(_amazonS3Mock.Object, _transferUtilityMock.Object);
    }

    [Fact]
    public async Task CreateBucketAsync_ShouldCreateBucket_WhenBucketDoesNotExist()
    {
        // Arrange
        const string bucketName = "test-bucket";
        _amazonS3Mock.Setup(x => x.ListBucketsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListBucketsResponse
            {
                Buckets = []
            });

        // Act
        await _bucketS3Service.CreateBucketAsync(bucketName, CancellationToken.None);

        // Assert
        _amazonS3Mock.Verify(x => x.PutBucketAsync(It.Is<string>(b => b == bucketName), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateBucketAsync_ShouldThrowConflictException_WhenBucketExists()
    {
        // Arrange
        const string bucketName = "test-bucket";
        _amazonS3Mock.Setup(x => x.ListBucketsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListBucketsResponse
            {
                Buckets = [new S3Bucket { BucketName = bucketName }]
            });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ConflictException>(() =>
            _bucketS3Service.CreateBucketAsync(bucketName, CancellationToken.None));
        Assert.Equal($"Bucket {bucketName} already exists.", exception.Message);
    }

    [Fact]
    public async Task ListBucketsAsync_ShouldReturnListOfBuckets()
    {
        // Arrange
        const string bucketName = "test-bucket";
        _amazonS3Mock.Setup(x => x.ListBucketsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListBucketsResponse
            {
                Buckets = [new S3Bucket { BucketName = bucketName }]
            });

        // Act
        var result = await _bucketS3Service.ListBucketsAsync(CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal(bucketName, result.First().BucketName);
    }

    [Fact]
    public async Task DeleteBucketAsync_ShouldDeleteAllObjectsAndBucket()
    {
        // Arrange
        const string bucketName = "test-bucket";

        _amazonS3Mock
            .Setup(x => x.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListObjectsV2Response
            {
                S3Objects =
                [
                    new S3Object { Key = "test-key.txt" }
                ]
            });

        _amazonS3Mock.Setup(x => x.ListBucketsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListBucketsResponse
            {
                Buckets = [new S3Bucket { BucketName = bucketName }]
            });

        _amazonS3Mock
            .Setup(x => x.DeleteObjectsAsync(It.IsAny<DeleteObjectsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteObjectsResponse());

        _amazonS3Mock
            .Setup(x => x.DeleteBucketAsync(It.IsAny<DeleteBucketRequest>(), It.IsAny<CancellationToken>()));

        // Act
        await _bucketS3Service.DeleteBucketAsync(bucketName, CancellationToken.None);

        // Assert
        _amazonS3Mock.Verify(
            x => x.ListObjectsV2Async(It.Is<ListObjectsV2Request>(r => r.BucketName == bucketName),
                It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _amazonS3Mock.Verify(
            x => x.DeleteObjectsAsync(It.Is<DeleteObjectsRequest>(r => r.BucketName == bucketName),
                It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        _amazonS3Mock.Verify(
            x => x.DeleteBucketAsync(It.Is<DeleteBucketRequest>(r => r.BucketName == bucketName),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UploadFileAsync_ShouldUploadFile()
    {
        // Arrange
        var file = new FileModel("test-bucket", "test-key.txt");
        file.SetStream(new MemoryStream());
        file.SetContentType("text/plain");

        // Act
        await _bucketS3Service.UploadFileAsync(file, CancellationToken.None);

        // Assert
        _transferUtilityMock.Verify(
            x => x.UploadAsync(
                It.Is<TransferUtilityUploadRequest>(r =>
                    r.BucketName == file.BucketName && r.Key == file.Key && r.InputStream == file.Stream &&
                    r.ContentType == file.ContentType), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetFileAsync_ShouldReturnFile_WhenFileExists()
    {
        // Arrange
        var file = new FileModel("test-bucket", "test-key.txt");
        var getObjectResponse = new GetObjectResponse
        {
            ResponseStream = new MemoryStream(),
            Headers = { ContentType = "text/plain" }
        };
        _amazonS3Mock
            .Setup(x => x.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(getObjectResponse);

        // Act
        var result = await _bucketS3Service.GetFileAsync(file, CancellationToken.None);

        // Assert
        Assert.Equal(getObjectResponse.ResponseStream, result.Stream);
        Assert.Equal(getObjectResponse.Headers.ContentType, result.ContentType);
    }

    [Fact]
    public async Task GetFileAsync_ShouldThrowNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        var file = new FileModel("test-bucket", "test-key.txt");
        _amazonS3Mock
            .Setup(x => x.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("Not Found", ErrorType.Receiver, null, "NoSuchKey",
                HttpStatusCode.NotFound));

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _bucketS3Service.GetFileAsync(file, CancellationToken.None));
        Assert.Equal("Not Found", exception.Message);
    }

    [Fact]
    public async Task DeleteFileAsync_ShouldDeleteFile()
    {
        // Arrange
        var file = new FileModel("test-bucket", "test-key.txt");

        _amazonS3Mock.Setup(x =>
                x.DeleteObjectAsync(
                    It.Is<DeleteObjectRequest>(r => r.BucketName == file.BucketName && r.Key == file.Key),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteObjectResponse());

        // Act
        await _bucketS3Service.DeleteFileAsync(file, CancellationToken.None);

        // Assert
        _amazonS3Mock.Verify(
            x => x.DeleteObjectAsync(
                It.Is<DeleteObjectRequest>(r => r.BucketName == file.BucketName && r.Key == file.Key),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListFilesAsync_ShouldReturnListOfFilesAndContinuationToken()
    {
        // Arrange
        const string bucketName = "test-bucket";
        var fileModel = new FileModel(bucketName);
        const string continuationToken = "next-token";
        var s3Objects = new List<S3Object>
        {
            new() { Key = "file1" },
            new() { Key = "file2" }
        };

        _amazonS3Mock.Setup(x => x.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListObjectsV2Response
            {
                S3Objects = s3Objects,
                NextContinuationToken = continuationToken
            });

        // Act
        var result = await _bucketS3Service.ListFilesAsync(fileModel, 10, CancellationToken.None);

        // Assert
        Assert.Equal(s3Objects.Count, result.Item1.Count);
        Assert.Equal(continuationToken, result.Item2);
    }

    [Fact]
    public async Task GetUrlFileAsync_ShouldReturnPreSignedUrl()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string fileKey = "test-key.txt";
        var fileModel = new FileModel(bucketName, fileKey);
        const string preSignedUrl = "https://example.com/pre-signed-url";
        var expiration = DateTime.Now.AddHours(1);

        _amazonS3Mock.Setup(x =>
                x.GetPreSignedURL(It.Is<GetPreSignedUrlRequest>(r => r.BucketName == bucketName && r.Key == fileKey)))
            .Returns(preSignedUrl);

        // Act
        var result = await _bucketS3Service.GetUrlFileAsync(fileModel, CancellationToken.None);

        // Assert
        Assert.Equal(preSignedUrl, result.PreSignedUrl);
        Assert.Equal(fileKey, result.Key);
        Assert.Equal(expiration.Hour, result.UrlExpiration?.Hour);
    }
}