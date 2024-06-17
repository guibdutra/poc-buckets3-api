using Moq;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Handlers;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Handlers;

public class DeleteFileHandlerTests
{
    private readonly DeleteFileHandler _handler;
    private readonly Mock<IStorage> _storageMock;

    public DeleteFileHandlerTests()
    {
        _storageMock = new Mock<IStorage>();
        _handler = new DeleteFileHandler(_storageMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteFileAsync()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string key = "test-key";
        var command = new DeleteFileCommand(bucketName, key);
        var cancellationToken = new CancellationToken();

        _storageMock.Setup(s =>
                s.DeleteFileAsync(It.Is<FileModel>(f => f.BucketName == bucketName && f.Key == key), cancellationToken))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _storageMock.Verify(
            s => s.DeleteFileAsync(It.Is<FileModel>(f => f.BucketName == bucketName && f.Key == key),
                cancellationToken), Times.Once);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenStorageIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new DeleteFileHandler(null!));
        Assert.Equal("storage", exception.ParamName);
    }
}