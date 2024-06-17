using Moq;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Handlers;
using Poc.BucketS3.Domain.Interfaces;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Handlers;

public class DeleteBucketHandlerTests
{
    private readonly DeleteBucketHandler _handler;
    private readonly Mock<IStorage> _storageMock;

    public DeleteBucketHandlerTests()
    {
        _storageMock = new Mock<IStorage>();
        _handler = new DeleteBucketHandler(_storageMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteBucketAsync()
    {
        // Arrange
        const string bucketName = "test-bucket";
        var command = new DeleteBucketCommand(bucketName);
        var cancellationToken = new CancellationToken();

        _storageMock.Setup(s => s.DeleteBucketAsync(bucketName, cancellationToken))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _storageMock.Verify(s => s.DeleteBucketAsync(bucketName, cancellationToken), Times.Once);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenStorageIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new DeleteBucketHandler(null!));
        Assert.Equal("storage", exception.ParamName);
    }
}