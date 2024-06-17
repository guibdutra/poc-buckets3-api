using Moq;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Handlers;
using Poc.BucketS3.Domain.Interfaces;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Handlers;

public class CreateBucketHandlerTests
{
    private readonly CreateBucketHandler _handler;
    private readonly Mock<IStorage> _storageMock;

    public CreateBucketHandlerTests()
    {
        _storageMock = new Mock<IStorage>();
        _handler = new CreateBucketHandler(_storageMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallCreateBucketAsync()
    {
        // Arrange
        const string bucketName = "test-bucket";
        var command = new CreateBucketCommand(bucketName);
        var cancellationToken = new CancellationToken();

        _storageMock.Setup(s => s.CreateBucketAsync(bucketName, cancellationToken))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _storageMock.Verify(s => s.CreateBucketAsync(bucketName, cancellationToken), Times.Once);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenStorageIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new CreateBucketHandler(null!));
        Assert.Equal("storage", exception.ParamName);
    }
}