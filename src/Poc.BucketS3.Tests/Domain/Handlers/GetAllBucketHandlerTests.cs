using Moq;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Handlers;
using Poc.BucketS3.Domain.Interfaces;
using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Handlers;

public class GetAllBucketHandlerTests
{
    private readonly GetAllBucketHandler _handler;
    private readonly Mock<IStorage> _storageMock;

    public GetAllBucketHandlerTests()
    {
        _storageMock = new Mock<IStorage>();
        _handler = new GetAllBucketHandler(_storageMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallListBucketsAsync()
    {
        // Arrange
        var command = new GetAllBucketCommand();
        var cancellationToken = new CancellationToken();
        var bucketList = new List<BucketModel>
        {
            new("bucket1"),
            new("bucket2")
        };

        _storageMock.Setup(s => s.ListBucketsAsync(cancellationToken))
            .ReturnsAsync(bucketList)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.Equal(bucketList, result);
        _storageMock.Verify(s => s.ListBucketsAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenStorageIsNull()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new GetAllBucketHandler(null!));
        Assert.Equal("storage", exception.ParamName);
    }
}