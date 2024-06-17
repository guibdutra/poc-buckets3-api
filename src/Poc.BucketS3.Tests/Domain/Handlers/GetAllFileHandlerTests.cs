using Moq;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Handlers;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Handlers;

public class GetAllFileHandlerTests
{
    private readonly GetAllFileHandler _handler;
    private readonly Mock<IStorage> _storageMock;

    public GetAllFileHandlerTests()
    {
        _storageMock = new Mock<IStorage>();
        _handler = new GetAllFileHandler(_storageMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallListFilesAsync()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const int maxKeys = 10;
        const string prefix = "prefix";
        const string continuationToken = "token";
        var command = new GetAllFileCommand(bucketName, maxKeys, prefix, continuationToken);
        var cancellationToken = new CancellationToken();

        var fileList = new List<FileModel>
        {
            new("bucket1"),
            new("bucket2")
        };

        _storageMock.Setup(s => s.ListFilesAsync(
                It.Is<FileModel>(f => f.BucketName == bucketName),
                maxKeys,
                cancellationToken,
                continuationToken,
                prefix))
            .ReturnsAsync((fileList, "next-token"))
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.Equal(fileList, result.Item1);
        Assert.Equal("next-token", result.Item2);
        _storageMock.Verify(s => s.ListFilesAsync(
                It.Is<FileModel>(f => f.BucketName == bucketName),
                maxKeys,
                cancellationToken,
                continuationToken,
                prefix),
            Times.Once);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenStorageIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new GetAllFileHandler(null!));
        Assert.Equal("storage", exception.ParamName);
    }
}