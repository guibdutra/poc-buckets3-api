# Poc.BucketS3 API

## Overview

The Poc.BucketS3 API is a sample project that demonstrates the implementation of S3 bucket and file operations using
.NET. The API provides endpoints for creating, retrieving, and deleting S3 buckets as well as uploading and downloading
files from these buckets.

## Features

- Create, retrieve, and delete S3 buckets.
- Upload and download files to/from S3 buckets.
- Logging and error handling.
- API versioning.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [AWS Account](https://aws.amazon.com/)
- [AWS CLI](https://aws.amazon.com/cli/)
- AWS credentials configured locally (usually in `~/.aws/credentials`).

## Getting Started

### Setup

1. **Clone the repository**
    ```sh
    git clone <repository-url>
    cd poc-buckets3-api
    ```

2. **Restore dependencies**
    ```sh
    dotnet restore
    ```

3. **Update AWS Configuration**
   Ensure your AWS credentials are properly configured. You can update the `launchSettings.json` file with your AWS
   configuration:
    ```json
   {
      "profiles": {
        "Poc.BucketS3.Api": {
          "commandName": "Project",
          "dotnetRunMessages": true,
          "launchBrowser": false,
          "launchUrl": "swagger",
          "applicationUrl": "https://localhost:5001;http://localhost:5000",
          "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development",
            "ENVIRONMENT": "dev",
            "LOG_LEVEL": "Debug",
            "AWS_REGION": "us-east-1",
            "AWS_USER_KEY": "user-key",
            "AWS_USER_SECRET": "user-secret"
          }
        }
      }
    }

    ```

4. **Build the project**
    ```sh
    dotnet build
    ```

5. **Run the project**
    ```sh
    dotnet run --project src/Poc.BucketS3.Api
    ```

### Running Tests

To run the tests, use the following command:

```sh
dotnet test
```

## Usage

### Endpoints

#### BucketsController

- **Create a bucket**
    ```
    POST /api/v1/buckets/create
    ```
  Request body:
    ```json
    [
      {
        "BucketName": "example-bucket"
      }
    ]
    ```
  Description: This endpoint allows the creation of one or more S3 buckets. Each bucket's name is specified in the
  request body.

- **Get all buckets**
    ```
    GET /api/v1/buckets/get-all
    ```
  Description: This endpoint retrieves a list of all S3 buckets.

- **Delete a bucket**
    ```
    DELETE /api/v1/buckets/delete?bucketName={bucketName}
    ```
  Description: This endpoint deletes the specified S3 bucket.

#### FilesController

- **Upload files**
    ```
    POST /api/v1/files/upload?bucketName={bucketName}
    ```
  Request form-data:
    ```
    files: [file1, file2, ...]
    ```
  Description: This endpoint uploads one or more files to the specified S3 bucket. The bucket name is specified as a
  query parameter and the files are included in the form data.

- **Download a file**
    ```
    GET /api/v1/files/download?bucketName={bucketName}&key={fileKey}
    ```
  Description: This endpoint downloads a file from the specified S3 bucket. The bucket name and file key are specified
  as query parameters.

- **List files in a bucket**
    ```
    GET /api/v1/files/get-all?bucketName={bucketName}
    ```
  Description: This endpoint lists all files in the specified S3 bucket. The bucket name is specified as a query
  parameter.

- **Delete a file**
    ```
    DELETE /api/v1/files/delete?bucketName={bucketName}&key={fileKey}
    ```
  Description: This endpoint deletes a file from the specified S3 bucket. The bucket name and file key are specified as
  query parameters.

- **Get URL files in a bucket**
    ```
    GET /api/v1/files/get-url?bucketName={bucketName}&key={fileKey}
    ```
  Description: This endpoint lists all files in the specified S3 bucket. The bucket name is specified as a query
  parameter.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License.
