using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Poc.BucketS3.Api.Controllers;
using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Api.Controllers;

public class FilesControllerTests
{
    private const string FileName = "test-file.txt";
    private const string ContentType = "text/plain";
    private const string BucketName = "bucket-test";
    private readonly FilesController _controller;

    private readonly Mock<ILogger<FilesController>> _loggerMock;
    private readonly Mock<IMediator> _mediatorMock;

    public FilesControllerTests()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-Example"] = "test-header";

        _loggerMock = new Mock<ILogger<FilesController>>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new FilesController(_loggerMock.Object, _mediatorMock.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = httpContext }
        };
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenDependenciesAreNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FilesController(null!, _mediatorMock.Object));
        Assert.Throws<ArgumentNullException>(() => new FilesController(_loggerMock.Object, null!));
    }

    [Fact]
    public async Task UploadFilesAsync_ReturnsNoContent()
    {
        // Arrange
        var files = new List<IFormFile> { new Mock<IFormFile>().Object };

        _mediatorMock.Setup(m => m.Send(It.IsAny<UploadFilesCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UploadFilesAsync(BucketName, files, Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UploadFilesAsync_ThrowsArgumentNullException_WhenFilesAreEmpty()
    {
        // Arrange
        var files = new List<IFormFile>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _controller.UploadFilesAsync(BucketName, files, Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task GetFileAsync_ReturnsFileResult()
    {
        // Arrange
        var formFile = await FormFileMock(FileName, ContentType);
        var stream = formFile.OpenReadStream();
        var fileModel = new FileModel(FileName, BucketName);
        fileModel.SetStream(stream);
        fileModel.SetContentType(ContentType);

        _mediatorMock.Setup(m => m.Send(It.IsAny<DownloadFileCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fileModel);

        // Act
        var result = await _controller.GetFileAsync(BucketName, FileName, Guid.NewGuid(), CancellationToken.None);

        // Assert
        var fileResult = Assert.IsType<FileStreamResult>(result);
        Assert.Equal(fileModel.FileName, fileResult.FileDownloadName);
        Assert.Equal(fileModel.ContentType, fileResult.ContentType);
        Assert.Same(stream, fileResult.FileStream);
        Assert.Equal($"attachment; filename={fileModel.FileName}", _controller.Response.Headers.ContentDisposition);
    }

    [Fact]
    public async Task GetFileAsync_ThrowsValidationException_WhenKeyIsEmpty()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() =>
            _controller.GetFileAsync(BucketName, string.Empty, Guid.NewGuid(), CancellationToken.None));
    }


    [Fact]
    public async Task DeleteFileAsync_ReturnsNoContent()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteFileCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteFileAsync(BucketName, FileName, Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetAllFileAsync_ReturnsOkObjectResult()
    {
        // Arrange
        var listObjects = new List<FileModel> { new(FileName, BucketName) };
        var listFilesResponse = new List<FileModel>(listObjects);
        const string token = "token";

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllFileCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((listFilesResponse, token));

        // Act
        var result = await _controller.GetAllFileAsync(BucketName, null, Guid.NewGuid(), CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(listObjects, okResult.Value);
    }

    [Fact]
    public async Task GetUrlFileAsync_ReturnsOkObjectResult()
    {
        // Arrange
        const string preSignedUrl = "https://example.com/pre-signed-url";
        var expectedObjectModel =
            new FileModel(BucketName, FileName, preSignedUrl, new DateTime(2022, 1, 2));

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUrlFileCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedObjectModel);

        // Act
        var result = await _controller.GetUrlFileAsync(BucketName, FileName, Guid.NewGuid(), CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedObjectModel, okResult.Value);
    }

    private static async Task<FormFile> FormFileMock(string fileName, string contentType)
    {
        const string content = "Hello World from a Fake File";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;

        var formFile = new FormFile(stream, 0, stream.Length, "id_from_form", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

        return formFile;
    }
}