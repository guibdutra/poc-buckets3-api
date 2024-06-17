using Moq;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Handlers;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Handlers;

public class DownloadFileHandlerTests
{
    private readonly DownloadFileHandler _handler;
    private readonly Mock<IStorage> _storageMock;

    public DownloadFileHandlerTests()
    {
        _storageMock = new Mock<IStorage>();
        _handler = new DownloadFileHandler(_storageMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallGetFileAsync()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string key = "test-key";
        var command = new DownloadFileCommand(bucketName, key);
        var cancellationToken = new CancellationToken();
        var fileModel = new FileModel(bucketName, key);

        _storageMock.Setup(s =>
                s.GetFileAsync(It.Is<FileModel>(f => f.BucketName == bucketName && f.Key == key), cancellationToken))
            .ReturnsAsync(fileModel)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.Equal(fileModel, result);
        _storageMock.Verify(
            s => s.GetFileAsync(It.Is<FileModel>(f => f.BucketName == bucketName && f.Key == key), cancellationToken),
            Times.Once);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenStorageIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new DownloadFileHandler(null!));
        Assert.Equal("storage", exception.ParamName);
    }
}