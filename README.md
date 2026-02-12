# Poc.BucketS3 API - AWS S3 CRUD with .NET 8

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)]()
[![License](https://img.shields.io/badge/license-MIT-blue.svg)]()
[![SOLID](https://img.shields.io/badge/SOLID-Principles-green)]()
[![DDD](https://img.shields.io/badge/DDD-Patterns-orange)]()

## 🎯 Overview

A **production-ready** RESTful API demonstrating **best practices** for AWS S3 operations using **.NET 8**, implementing **SOLID principles** and **Domain-Driven Design (DDD)** patterns.

This project serves as a **reference implementation** for building clean, maintainable, and testable .NET Web APIs.

### ✨ Key Features

- ✅ **SOLID Principles** - Clean architecture with clear separation of concerns
- ✅ **Domain-Driven Design** - Value Objects, Domain Exceptions, CQRS pattern
- ✅ **CQRS with MediatR** - Command/Query separation for better maintainability
- ✅ **Comprehensive Testing** - Unit tests with high coverage
- ✅ **AWS S3 Integration** - Full CRUD operations (Create, Read, Update, Delete)
- ✅ **Error Handling** - Centralized exception middleware with proper HTTP status codes
- ✅ **API Versioning** - Versioned endpoints for backward compatibility
- ✅ **Swagger/OpenAPI** - Interactive API documentation
- ✅ **Resource Management** - Proper disposal patterns to prevent memory leaks
- ✅ **Type Safety** - Value Objects for domain validation

---

## 📚 Documentation

- **[SOLID & DDD Guide](./SOLID_DDD_GUIDE.md)** - Detailed architecture explanation
- **[Code Review Report](./CODE_REVIEW_REPORT.md)** - Recent improvements and bug fixes

---

## 🏗️ Architecture

This project follows a **layered architecture** with clear separation of concerns:

```
┌────────────────────────────────────────┐
│   Presentation Layer (API)             │  ← Controllers, Middlewares
├────────────────────────────────────────┤
│   Application Layer (Domain)           │  ← Commands, Handlers, Models
├────────────────────────────────────────┤
│   Infrastructure Layer                 │  ← AWS S3 Service Implementation
└────────────────────────────────────────┘
```

**Key Patterns:**
- **Value Objects**: `BucketName`, `FileName`, `ContentType`, `PreSignedUrl`
- **CQRS**: Command/Query separation using MediatR
- **Repository Pattern**: `IStorage` abstraction over AWS S3
- **Exception Handling**: Domain exceptions + centralized middleware

---

## 🚀 Features

### Bucket Operations
- Create S3 buckets with validation
- List all buckets
- Delete buckets

### File Operations  
- Upload multiple files with stream management
- Download files with proper resource disposal
- List files with pagination support
- Delete files
- Generate pre-signed URLs for temporary access

### Cross-Cutting Concerns
- Correlation ID tracking for distributed tracing
- Centralized exception handling with detailed error responses
- Request/response logging
- Swagger UI for API exploration

---

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
## 📋 API Endpoints

### 🗂️ Bucket Operations

#### Create Buckets
```http
POST /api/v1/buckets/create
Content-Type: application/json

[
  { "BucketName": "my-data-bucket" },
  { "BucketName": "my-backup-bucket" }
]
```

#### List All Buckets
```http
GET /api/v1/buckets/get-all
```

#### Delete Bucket
```http
DELETE /api/v1/buckets/delete?bucketName=my-data-bucket
```

---

### 📁 File Operations

#### Upload Files
```http
POST /api/v1/files/upload?bucketName=my-data-bucket
Content-Type: multipart/form-data

files: [file1.pdf, file2.jpg, ...]
```

#### Download File
```http
GET /api/v1/files/download?bucketName=my-data-bucket&key=document.pdf
```

#### List Files (with pagination)
```http
GET /api/v1/files/get-all?bucketName=my-data-bucket&maxKeys=100&prefix=folder/
```

#### Delete File
```http
DELETE /api/v1/files/delete?bucketName=my-data-bucket&key=document.pdf
```

#### Get Pre-Signed URL
```http
GET /api/v1/files/get-url?bucketName=my-data-bucket&key=document.pdf
```

**Response:**
```json
{
  "bucketName": "my-data-bucket",
  "key": "document.pdf",
  "preSignedUrl": "https://s3.amazonaws.com/...",
  "expiresAt": "2024-01-15T10:30:00Z"
}
```

---

## 🧪 Testing

Run all tests:
```bash
dotnet test
```

Run specific test category:
```bash
dotnet test --filter "Category=Domain"
dotnet test --filter "Category=Integration"
```

**Test Coverage:**
- ✅ Domain Models & Value Objects
- ✅ Command Handlers (CQRS)
- ✅ Infrastructure Services
- ✅ API Controllers
- ✅ Exception Handling

---

## 📊 Code Quality

### SOLID Compliance: **8/10**
- ✅ Single Responsibility Principle
- ✅ Open/Closed Principle
- ✅ Liskov Substitution Principle
- ✅ Interface Segregation Principle
- ✅ Dependency Inversion Principle

### DDD Patterns Implemented:
- ✅ Value Objects (`BucketName`, `FileName`, `ContentType`, `PreSignedUrl`)
- ✅ Domain Exceptions
- ✅ CQRS with MediatR
- ✅ Repository Pattern
- ✅ Layered Architecture

---

## 🔒 Security Best Practices

- ✅ AWS credentials via environment variables (not hardcoded)
- ✅ `launchSettings.json` excluded from version control
- ✅ Input validation at domain level
- ✅ Pre-signed URLs with expiration
- ✅ Proper exception handling (no stack trace leaks)

---

## 🐛 Recent Improvements

See [CODE_REVIEW_REPORT.md](./CODE_REVIEW_REPORT.md) for details on recent bug fixes:

- ✅ Fixed Release build failure (missing namespaces)
- ✅ Fixed resource leaks (HTTP connections, file streams)
- ✅ Fixed API consistency (parameter binding)
- ✅ Fixed DateTime.Now → DateTime.UtcNow for AWS
- ✅ Added input validation (maxKeys range)
- ✅ Improved error responses (HTTP 499 for cancellation)

---

## 👥 Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Follow SOLID principles
- Add unit tests for new features
- Update documentation
- Keep test coverage > 80%

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](./LICENSE) file for details.

---

## 👨‍💻 Autor

**Guilherme Dutra**

- 📧 Email: [guibdutra@icloud.com](mailto:guibdutra@icloud.com)
- 💼 LinkedIn: [linkedin.com/in/guibdutra](https://www.linkedin.com/in/guibdutra/)
- 🐙 GitHub: [@guibdutra](https://github.com/guibdutra)

---

## 🙏 Acknowledgments

- Clean Architecture principles by Robert C. Martin
- Domain-Driven Design by Eric Evans
- CQRS pattern implementation via MediatR
- AWS SDK for .NET team

---

**⭐ If this project helped you, please give it a star!**
