using Poc.BucketS3.Domain.Commands;
using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Commands;

public class UploadFilesCommandTests
{
    [Fact]
    public void UploadFilesCommand_ShouldInitializeCorrectly()
    {
        // Arrange
        var fileList = new List<FileModel>
        {
            new("test-bucket-1", "test-key1.pdf", "test-key1.pdf", new MemoryStream([0x01, 0x02, 0x03]),
                "application/pdf"),
            new("test-bucket-2", "test-key2.pdf", "test-key2.pdf", new MemoryStream([0x01, 0x02, 0x03]),
                "application/pdf")
        };

        // Act
        var command = new UploadFilesCommand(fileList);

        // Assert
        Assert.Equal(fileList, command.ListFile);
    }

    [Fact]
    public void UploadFilesCommand_ListFile_ShouldNotBeNull()
    {
        // Arrange
        var fileList = new List<FileModel>
        {
            new("test-bucket", "test-key.pdf", "test-key.pdf", new MemoryStream([0x01, 0x02, 0x03]), "application/pdf")
        };

        // Act
        var command = new UploadFilesCommand(fileList);

        // Assert
        Assert.NotNull(command.ListFile);
    }

    [Fact]
    public void UploadFilesCommand_ShouldAllowEmptyFileList()
    {
        // Arrange & Act
        var command = new UploadFilesCommand(new List<FileModel>());

        // Assert
        Assert.Empty(command.ListFile);
    }
}