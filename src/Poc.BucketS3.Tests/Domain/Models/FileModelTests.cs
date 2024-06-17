using Poc.BucketS3.Domain.Models;
using Xunit;

namespace Poc.BucketS3.Tests.Domain.Models;

public class FileModelTests
{
    [Fact]
    public void FileModel_ShouldInitializeWithBucketName()
    {
        // Arrange
        var bucketName = "test-bucket";

        // Act
        var model = new FileModel(bucketName);

        // Assert
        Assert.Equal(bucketName, model.BucketName);
    }

    [Fact]
    public void FileModel_ShouldInitializeWithBucketNameAndFileName()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string fileName = "test-file";

        // Act
        var model = new FileModel(bucketName, fileName);

        // Assert
        Assert.Equal(bucketName, model.BucketName);
        Assert.Equal(fileName, model.Key);
        Assert.Equal(fileName, model.FileName);
    }

    [Fact]
    public void FileModel_ShouldInitializeWithAllParameters()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string key = "test-key";
        const string fileName = "test-file";
        Stream stream = new MemoryStream();
        const string contentType = "text/plain";

        // Act
        var model = new FileModel(bucketName, key, fileName, stream, contentType);

        // Assert
        Assert.Equal(bucketName, model.BucketName);
        Assert.Equal(key, model.Key);
        Assert.Equal(fileName, model.FileName);
        Assert.Equal(stream, model.Stream);
        Assert.Equal(contentType, model.ContentType);
    }

    [Fact]
    public void FileModel_ShouldInitializeWithPreSignedUrl()
    {
        // Arrange
        const string bucketName = "test-bucket";
        const string fileName = "test-file";
        const string preSignedUrl = "https://example.com/pre-signed-url";
        var expiration = DateTime.UtcNow.AddHours(1);

        // Act
        var model = new FileModel(bucketName, fileName, preSignedUrl, expiration);

        // Assert
        Assert.Equal(bucketName, model.BucketName);
        Assert.Equal(fileName, model.Key);
        Assert.Equal(fileName, model.FileName);
        Assert.Equal(preSignedUrl, model.PreSignedUrl);
        Assert.Equal(expiration, model.UrlExpiration);
    }

    [Fact]
    public void FileModel_ShouldAllowSettingStream()
    {
        // Arrange
        const string bucketName = "test-bucket";
        var model = new FileModel(bucketName);
        Stream stream = new MemoryStream();

        // Act
        model.SetStream(stream);

        // Assert
        Assert.Equal(stream, model.Stream);
    }

    [Fact]
    public void FileModel_ShouldAllowSettingContentType()
    {
        // Arrange
        const string bucketName = "test-bucket";
        var model = new FileModel(bucketName);
        const string contentType = "text/plain";

        // Act
        model.SetContentType(contentType);

        // Assert
        Assert.Equal(contentType, model.ContentType);
    }
}