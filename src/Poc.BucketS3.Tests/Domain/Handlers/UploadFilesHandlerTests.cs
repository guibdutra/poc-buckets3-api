using Moq;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Handlers;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Handlers;

public class UploadFilesHandlerTests
{
    private readonly UploadFilesHandler _handler;
    private readonly Mock<IStorage> _storageMock;

    public UploadFilesHandlerTests()
    {
        _storageMock = new Mock<IStorage>();
        _handler = new UploadFilesHandler(_storageMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallUploadFileAsyncForEachFile()
    {
        // Arrange
        var files = new List<FileModel>
        {
            new("test-bucket-1", "test-key1.pdf", "test-key1.pdf", new MemoryStream([0x01, 0x02, 0x03]),
                "application/pdf"),
            new("test-bucket-2", "test-key2.pdf", "test-key2.pdf", new MemoryStream([0x01, 0x02, 0x03]),
                "application/pdf")
        };
        var command = new UploadFilesCommand(files);
        var cancellationToken = new CancellationToken();

        _storageMock.Setup(s => s.UploadFileAsync(It.IsAny<FileModel>(), cancellationToken))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _storageMock.Verify(s => s.UploadFileAsync(It.IsAny<FileModel>(), cancellationToken),
            Times.Exactly(files.Count));
        _storageMock.Verify(
            s => s.UploadFileAsync(It.Is<FileModel>(f => f.FileName == "test-key1.pdf"), cancellationToken),
            Times.Once);
        _storageMock.Verify(
            s => s.UploadFileAsync(It.Is<FileModel>(f => f.FileName == "test-key2.pdf"), cancellationToken),
            Times.Once);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenStorageIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new UploadFilesHandler(null!));
        Assert.Equal("storage", exception.ParamName);
    }
}