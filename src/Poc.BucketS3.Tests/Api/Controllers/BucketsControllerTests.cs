using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Poc.BucketS3.Api.Controllers;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Api.Controllers;

public class BucketsControllerTests
{
    private readonly BucketsController _controller;
    private readonly Mock<ILogger<BucketsController>> _loggerMock;
    private readonly Mock<IMediator> _mediatorMock;

    public BucketsControllerTests()
    {
        _loggerMock = new Mock<ILogger<BucketsController>>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new BucketsController(_loggerMock.Object, _mediatorMock.Object);
    }


    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenDependenciesAreNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BucketsController(null!, _mediatorMock.Object));
        Assert.Throws<ArgumentNullException>(() => new BucketsController(_loggerMock.Object, null!));
    }

    [Fact]
    public async Task CreateBucketAsync_ReturnsNoContent()
    {
        // Arrange
        var bucketList = new List<BucketModel>
        {
            new("test-bucket")
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateBucketCommand>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await _controller.CreateBucketAsync(bucketList, Guid.NewGuid(), CancellationToken.None);

        // Assert
        var actionResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, actionResult.StatusCode);
    }

    [Fact]
    public async Task GetAllBucketAsync_ReturnsOkResult()
    {
        // Arrange
        var buckets = new List<BucketModel>
        {
            new("test-bucket-1"),
            new("test-bucket-2")
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllBucketCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(buckets);

        // Act
        var result = await _controller.GetAllBucketAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<BucketModel>>(okResult.Value);
        Assert.Equal(buckets.Count, returnValue.Count);
    }

    [Fact]
    public async Task DeleteBucketAsync_ReturnsNoContent()
    {
        // Arrange
        const string bucketName = "test-bucket";

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteBucketCommand>(), It.IsAny<CancellationToken>()));

        // Act
        var result = await _controller.DeleteBucketAsync(bucketName, Guid.NewGuid(), CancellationToken.None);

        // Assert
        var actionResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, actionResult.StatusCode);
    }
}