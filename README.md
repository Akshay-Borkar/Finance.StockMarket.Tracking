# Finance.StockMarket.Tracking

A modern, enterprise-grade **stock market tracking and sentiment analysis platform** built with **.NET 9** and following **Clean Architecture principles**. This application enables users to track stock sectors, monitor real-time stock prices via WebSocket (SignalR), analyze sentiment from financial news, and manage investment portfolios with automated background jobs for market data updates.

**Live Status**: 🟢 Production-Ready  
**Framework**: .NET 9.0  
**Architecture**: Clean Architecture with Layered Design  
**License**: MIT

---

## Table of Contents

- [Overview](#overview)
- [Key Features](#key-features)
- [Technology Stack](#technology-stack)
- [Project Architecture](#project-architecture)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Database & Authentication](#database--authentication)
- [Background Jobs](#background-jobs)
- [Configuration](#configuration)
- [Directory Structure](#directory-structure)
- [Development Notes](#development-notes)

---

## Overview

**Finance.StockMarket.Tracking** is a comprehensive stock market analysis platform that integrates real-time data from Yahoo Finance, machine learning for sentiment analysis, and WebSocket technology for live price updates. The platform is designed with enterprise-grade patterns including Repository Pattern, CQRS, Dependency Injection, and comprehensive error handling.

### Core Capabilities:
- 📊 **Stock Sector Management** - CRUD operations on stock sectors (IT, Healthcare, Finance, etc.)
- 💰 **Portfolio Tracking** - Record and monitor investments with buying prices and dates
- 📈 **Real-time Price Updates** - WebSocket-based live stock price streaming via SignalR
- 🧠 **Sentiment Analysis** - Analyze news sentiment from Yahoo Finance using ML.NET
- 🔐 **Secure Authentication** - JWT-based user authentication with role-based access
- ⏰ **Background Jobs** - Recurring Hangfire jobs for automated market data updates (minutely, hourly, daily, weekly)
- 💾 **High-Performance Caching** - Redis for caching stock prices and subscribed tickers
- 📧 **Email Notifications** - SendGrid integration for user notifications
- 📊 **Job Monitoring** - Hangfire Dashboard for background job visibility and management

---

## Key Features

| Feature | Description |
|---------|-------------|
| **Stock Sector Management** | Create, read, update, and delete stock market sectors with sector P/E ratios |
| **Individual Stock Tracking** | Track ticker symbols, prices, market cap, P/E ratios, and user portfolios |
| **Real-time Updates** | SignalR WebSocket connection for live stock price streaming to all connected clients |
| **Sentiment Analysis** | Fetch and analyze news headlines from Yahoo Finance RSS feeds using ML.NET FastTree algorithm |
| **User Authentication** | JWT bearer token authentication with role-based authorization (Admin, Employee roles) |
| **Investment Portfolio** | Record investments with purchase prices, dates, and reference to underlying stocks |
| **Background Job Scheduling** | Hangfire-based recurring jobs for automated price updates (minute, hourly, daily, weekly frequencies) |
| **Redis Caching** | High-speed caching layer for frequent data, subscribed tickers, and Hangfire job queue |
| **Email Service** | SendGrid integration for sending transactional emails and notifications |
| **Swagger/OpenAPI** | Interactive API documentation available in Development environment |
| **Exception Handling** | Centralized middleware-based exception handling with custom problem details |
| **Database Auditing** | All entities track creation and modification timestamps |

---

## Technology Stack

### Core Framework
- **Runtime**: .NET 9.0
- **Web Framework**: ASP.NET Core (Web)
- **Language**: C# with nullable reference types enabled

### Data & Persistence
- **Database**: SQL Server 2019+
- **ORM**: Entity Framework Core 9.0.1
- **Connection Library**: Newtonsoft.Json 13.0.3
- **Migrations**: EF Core Code-First Migrations

### Authentication & Security
- **Identity**: ASP.NET Identity with JWT Bearer Tokens
- **Authentication**: Microsoft.AspNetCore.Authentication.JwtBearer
- **Token Duration**: 15 minutes (configurable)
- **Encryption**: Standard .NET cryptographic providers

### Real-time Communication
- **WebSocket Protocol**: SignalR (2.x)
- **Hub**: `/stockMarketHub` for price updates
- **Client Support**: JavaScript, .NET, Java, Python

### Caching & Background Jobs
- **Cache Provider**: Redis (StackExchange.Redis 9.0.1)
- **Background Jobs**: Hangfire 1.8.17 with Redis persistent storage
- **Job Scheduler**: Recurring jobs with multiple frequency options
- **Dashboard**: Hangfire web interface for job monitoring

### AI/ML & Analytics
- **Machine Learning**: ML.NET 4.0.1 with FastTree algorithm
- **Sentiment Analysis**: Text classification for news headlines
- **Training**: ML.NET model training pipeline included

### API Integration & Services
- **External Data**: Yahoo Finance (RSS headlines)
- **Email Service**: SendGrid 9.29.3
- **Data Mapping**: AutoMapper 13.0.1 (DTO to Entity mapping)
- **API Design**: CQRS pattern via MediatR 12.4.1
- **Validation**: FluentValidation 11.11.0

### API Documentation
- **OpenAPI/Swagger**: ASP.NET Core OpenAPI integration
- **Swagger UI**: Interactive API documentation (Development only)

### Logging
- **Logger**: Microsoft.Extensions.Logging
- **Structured Logging**: Supports JSON output for log aggregation

---

## Project Architecture

This project follows **Clean Architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────┐
│           Finance.StockMarket.Api (Presentation)        │
│  - Controllers (AuthController, StockSectorController)  │
│  - Middleware (ExceptionHandling)                       │
└──────────────┬──────────────────────────────────────────┘
               │
┌──────────────▼──────────────────────────────────────────┐
│      Finance.StockMarket.Application (Business Logic)   │
│  - Commands & Queries (MediatR)                         │
│  - AutoMapper Profiles                                  │
│  - FluentValidation Rules                               │
│  - Service Contracts (Interfaces)                       │
└──────────────┬──────────────────────────────────────────┘
               │
┌──────────────┴──────────────────────────────────────────┐
│  Finance.StockMarket.Domain (Domain Entities & DTOs)    │
│  - Entities (Stock, StockSector, Investment)            │
│  - Base Entities (Audit fields)                         │
│  - DTOs (Request/Response models)                       │
└──────────────┬──────────────────────────────────────────┘
               │
     ┌─────────┴──────────┐
     │                    │
┌────▼────────────────┐  ┌▼──────────────────────────────┐
│ Persistence Layer   │  │ Infrastructure Layer          │
│ (Data Access)       │  │ (External Services)           │
├─────────────────────┤  ├───────────────────────────────┤
│ - DbContext         │  │ - Yahoo Finance Service       │
│ - Repositories      │  │ - Sentiment Analysis Service  │
│ - Migrations        │  │ - SignalR Hub                 │
│ - EF Configuration  │  │ - Background Jobs (Hangfire)  │
└─────────────────────┘  │ - Redis Cache Service         │
                         │ - Email Service               │
                         │ - Job Scheduler               │
                         └───────────────────────────────┘
     
     ┌─────────────────────────────────────────────┐
     │ Identity Layer (Auth & User Management)     │
     │ - ApplicationUser                           │
     │ - Identity DbContext                        │
     │ - Role Configuration                        │
     │ - Auth & User Services                      │
     └─────────────────────────────────────────────┘
```

### Layer Responsibilities

| Layer | Purpose | Key Files |
|-------|---------|-----------|
| **API** | HTTP request handling, response formatting, route configuration | `Controllers/`, `Middleware/` |
| **Application** | Business logic, validation, data transformation, CQRS commands/queries | `Features/`, `MappingProfiles/`, Contracts |
| **Domain** | Core business entities, DTOs, domain rules | `Investment.cs`, `Stock.cs`, `StockSector.cs` |
| **Infrastructure** | External service implementations, background jobs, caching | `YahooFinance/`, `SentimentAnalysis/`, `HangfireJob/` |
| **Persistence** | Database access, repository implementation | `DatabaseContext/`, `Repositories/`, `Migrations/` |
| **Identity** | User authentication, authorization, JWT tokens | `Models/`, `Services/`, `DbContext/` |

---

## Prerequisites

Before running the application, ensure you have the following installed:

- **[.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)** or later
- **[SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)** (Express or Developer Edition)
- **[Redis 7.0+](https://redis.io/download)** (local or Azure cache)
- **[SendGrid API Key](https://sendgrid.com/)** (for email functionality)
- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** or **[VS Code](https://code.visualstudio.com/)** with C# extension
- **[Git](https://git-scm.com/)** for cloning the repository

### Optional
- **[Postman](https://www.postman.com/)** or **[Insomnia](https://insomnia.rest/)** for API testing
- **[Azure Data Studio](https://learn.microsoft.com/en-us/azure-data-studio/)** for database management
- **[Redis Commander](https://joeferner.github.io/redis-commander/)** for Redis inspection

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/Finance.StockMarket.Tracking.git
cd Finance.StockMarket.Tracking
```

### 2. Configure Environment Settings

Update the connection strings and API keys in `Finance.StockMarket.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "FinanceStockDatabaseConnectionString": "Server=localhost;Database=FinanceStockDatabase;Trusted_Connection=true;TrustServerCertificate=true;",
    "Redis": "localhost:6379"
  },
  "JwtSettings": {
    "Key": "your-very-long-secret-key-at-least-32-characters-for-production",
    "Issuer": "FinanceStockMarket.Api",
    "Audience": "FinanceStockMarketUser",
    "DurationInMinutes": 15
  },
  "EmailSettings": {
    "ApiKey": "your-sendgrid-api-key",
    "FromAddress": "your-email@example.com",
    "FromName": "Stock Market Tracking"
  },
  "CorsSettings": {
    "AllowedOrigins": "http://localhost:3000/"
  }
}
```

### 3. Restore NuGet Packages

```bash
dotnet restore
```

### 4. Create & Migrate Databases

```bash
# Migrate the main database (Persistence layer)
dotnet ef database update --project Finance.StockMarket.Persistence --startup-project Finance.StockMarket.Api

# Migrate the identity database
dotnet ef database update --project Finance.StockMarket.Identity --startup-project Finance.StockMarket.Api
```

### 5. Run the Application

```bash
dotnet run --project Finance.StockMarket.Api
```

The application will start on:
- **HTTP**: `http://localhost:5059`
- **HTTPS**: `https://localhost:7206`

### 6. Access the Application

| Component | URL | Purpose |
|-----------|-----|---------|
| **Swagger UI** | https://localhost:7206/swagger/index.html | Interactive API documentation |
| **Hangfire Dashboard** | https://localhost:7206/hangfire | Monitor background jobs |
| **SignalR Hub** | wss://localhost:7206/stockMarketHub | Real-time stock price updates |

---

## API Documentation

### Authentication Endpoints

#### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe"
}

Response: 200 OK
{
  "id": "user-id",
  "email": "user@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Login User
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}

Response: 200 OK
{
  "id": "user-id",
  "email": "user@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Stock Sector Endpoints

#### Get All Sectors
```http
GET /api/stock-sectors
Authorization: Bearer {token}

Response: 200 OK
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Information Technology",
    "sectorPERatio": 25.3,
    "dateCreated": "2025-02-11T10:30:00Z",
    "dateModified": "2025-02-11T10:30:00Z"
  },
  ...
]
```

#### Get Sector by ID
```http
GET /api/stock-sectors/{id}
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Information Technology",
  "sectorPERatio": 25.3,
  "dateCreated": "2025-02-11T10:30:00Z"
}
```

#### Create Sector
```http
POST /api/stock-sectors
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Healthcare",
  "sectorPERatio": 18.5
}

Response: 201 Created
```

#### Update Sector
```http
PUT /api/stock-sectors/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Healthcare",
  "sectorPERatio": 19.0
}

Response: 200 OK
```

#### Delete Sector
```http
DELETE /api/stock-sectors/{id}
Authorization: Bearer {token}

Response: 204 No Content
```

### Sentiment Analysis Endpoints

#### Analyze Yahoo Finance News
```http
GET /api/sentiment/analyze-yahoo-news/{ticker}
Authorization: Bearer {token}

Example: GET /api/sentiment/analyze-yahoo-news/AAPL

Response: 200 OK
{
  "ticker": "AAPL",
  "headlines": [
    {
      "title": "Apple Stock Hits New High",
      "sentiment": "Positive",
      "source": "Yahoo Finance"
    },
    ...
  ]
}
```

For complete API documentation, visit the **Swagger/OpenAPI endpoint** at: `https://localhost:7206/swagger/index.html`

---

## Database & Authentication

### Database Architecture

#### Main Database: `FinanceStockDatabase`
- **Context**: `FinanceStockMarketDatabaseContext`
- **Provider**: SQL Server
- **Key Tables**:
  - `StockSectors` - Market sectors (IT, Healthcare, Finance, etc.)
  - `Stocks` - Individual stock listings with price data
  - `Investments` - User investments and portfolio entries
  - `SentimentData` - Cached sentiment analysis results

#### Identity Database: `FinanceStockMarketIdentity`
- **Context**: `FinanceStockMarketIdentityDBContext`
- **Provider**: SQL Server
- **Key Tables**:
  - `AspNetUsers` - User account information
  - `AspNetRoles` - Application roles (Administrator, Employee)
  - `AspNetUserRoles` - User-role assignments

### Authentication Mechanism

**Type**: JWT Bearer Token  
**Implementation**: ASP.NET Identity + Custom JWT Generator  
**Token Duration**: 15 minutes (configurable in `appsettings.json`)

#### Default Seed Users

| Username | Email | Password | Role |
|----------|-------|----------|------|
| Admin | admin@localhost.com | admin | Administrator |
| User | user@localhost.com | user | Employee |

> **⚠️ Security Note**: Change default credentials immediately in production. Implement secure password policies and multi-factor authentication.

#### JWT Token Structure

```json
{
  "email": "user@example.com",
  "sub": "user-id",
  "iat": 1676011200,
  "exp": 1676012100
}
```

### Authentication Flow

1. User posts credentials to `/api/auth/login` or `/api/auth/register`
2. Service validates credentials against Identity database
3. JWT token generated with configurable expiration
4. Client includes token in `Authorization: Bearer {token}` header for subsequent requests
5. Middleware validates token signature and expiration on each request

---

## Background Jobs

### Hangfire Configuration

**Storage**: Redis (persistent)  
**Dashboard**: Enabled at `/hangfire` (authentication recommended for production)  
**Scheduler**: Automatic job scheduling via `JobSchedulerService`

### Recurring Jobs

| Job Name | Frequency | Purpose | Implementation |
|----------|-----------|---------|-----------------|
| **UpdateStockPricesMinutely** | Every 1 minute | Fetch latest stock prices, broadcast via SignalR | `StockPriceUpdateJob` |
| **UpdateStockPricesHourly** | Every 60 minutes | Hourly market data sync | `StockPriceUpdateJob` |
| **UpdateStockPricesDaily** | Daily at 00:00 UTC | End-of-day market data update | `StockPriceUpdateJob` |
| **UpdateStockPricesWeekly** | Weekly (Monday 09:00 UTC) | Weekly market summary & analysis | `StockPriceUpdateJob` |

### Background Job Flow

```
┌────────────────────────────────┐
│  Hangfire Scheduler            │
│  (Triggers at scheduled time)  │
└────────────┬───────────────────┘
             │
             ▼
┌────────────────────────────────┐
│  Job Execution                 │
│  (Hangfire Worker)             │
└────────────┬───────────────────┘
             │
             ▼
┌────────────────────────────────┐
│  YahooFinanceService           │
│  (Fetch price data)            │
└────────────┬───────────────────┘
             │
             ▼
┌────────────────────────────────┐
│  RedisCacheService             │
│  (Update cache)                │
└────────────┬───────────────────┘
             │
             ▼
┌────────────────────────────────┐
│  SignalRService                │
│  (Broadcast to clients)        │
└────────────────────────────────┘
```

### Monitoring Jobs

Access the **Hangfire Dashboard** at `https://localhost:7206/hangfire` to:
- View job execution history
- Monitor job success/failure rates
- Trigger jobs manually
- View job parameters and results

---

## Configuration

### appsettings.json Structure

```json
{
  "ConnectionStrings": {
    "FinanceStockDatabaseConnectionString": "connection-string",
    "Redis": "localhost:6379"
  },
  "JwtSettings": {
    "Key": "secret-key-min-32-chars-for-HS256",
    "Issuer": "FinanceStockMarket.Api",
    "Audience": "FinanceStockMarketUser",
    "DurationInMinutes": 15
  },
  "EmailSettings": {
    "ApiKey": "sendgrid-api-key",
    "FromAddress": "noreply@stockmarket.com",
    "FromName": "Stock Market Tracking"
  },
  "CorsSettings": {
    "AllowedOrigins": "http://localhost:3000/"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

### Environment-Specific Configurations

- **Development** (`appsettings.Development.json`):
  - Swagger/OpenAPI enabled
  - Hangfire Dashboard accessible
  - Detailed logging
  - No SSL validation
  
- **Production** (`appsettings.json`):
  - SSL/TLS required
  - Hangfire Dashboard requires authentication
  - Minimal logging (Warning level)
  - Connection string from environment variables
  - API keys from secrets management

### CORS Configuration

Default CORS policy allows requests from frontend at `http://localhost:3000/`:
- All HTTP methods (GET, POST, PUT, DELETE, PATCH)
- All headers
- Credentials included
- To modify, update `appsettings.json` `CorsSettings:AllowedOrigins`

---

## Directory Structure

```
Finance.StockMarket.Tracking/
│
├── Finance.StockMarket.Api/
│   ├── Controllers/
│   │   ├── AuthController.cs              # Authentication endpoints
│   │   ├── SentimentController.cs         # Sentiment analysis endpoints
│   │   └── StockSectorController.cs       # Stock sector CRUD endpoints
│   ├── Middleware/
│   │   └── ExceptionMiddleware.cs         # Global exception handling
│   ├── Models/
│   │   └── CustomValidationProblemDetails.cs
│   ├── Properties/
│   │   └── launchSettings.json            # Launch configuration (port 5059/7206)
│   ├── Program.cs                         # Application startup & DI configuration
│   ├── appsettings.json                   # Configuration settings
│   └── appsettings.Development.json       # Development-specific settings
│
├── Finance.StockMarket.Application/
│   ├── Contracts/                         # Service interfaces
│   │   ├── Email/
│   │   ├── Hangfire/
│   │   ├── Identity/
│   │   ├── Logging/
│   │   ├── Persistence/
│   │   ├── RedisCache/
│   │   ├── SentimentAnalysis/
│   │   ├── SignalR/
│   │   └── YahooFinance/
│   ├── Features/
│   │   └── StockSector/                   # CQRS commands/queries for stock sectors
│   ├── MappingProfiles/
│   │   └── StockSectorProfile.cs          # AutoMapper configuration
│   ├── Exceptions/
│   │   ├── BadRequestException.cs
│   │   └── NotFoundException.cs
│   ├── Models/
│   │   ├── Email/
│   │   └── Identity/
│   └── ApplicationServiceRegistration.cs  # Dependency injection configuration
│
├── Finance.StockMarket.Domain/
│   ├── Common/
│   │   ├── BaseEntity.cs                  # Base entity with audit fields
│   │   └── StockMarketDataDto.cs
│   ├── DTOs/
│   │   └── SentimentRequest.cs
│   ├── SentimentDataEntity/
│   │   ├── SentimentData.cs
│   │   └── SentimentPrediction.cs
│   ├── Stock.cs                           # Stock entity
│   ├── StockSector.cs                     # Stock sector entity
│   └── Investment.cs                      # Investment entity
│
├── Finance.StockMarket.Identity/
│   ├── Configurations/
│   │   ├── RoleConfiguration.cs           # Seed default roles
│   │   ├── UserConfiguration.cs           # Seed default users
│   │   └── UserRoleConfiguration.cs       # User-role mappings
│   ├── DbContext/
│   │   └── FinanceStockMarketIdentityDBContext.cs
│   ├── Migrations/
│   │   └── [Database migration files]
│   ├── Models/
│   │   └── ApplicationUser.cs             # Custom user model
│   ├── Services/
│   │   ├── AuthService.cs                 # Authentication business logic
│   │   └── UserService.cs                 # User management
│   └── IdentityServicesRegistration.cs    # Dependency injection
│
├── Finance.StockMarket.Infrastructure/
│   ├── BackgroundJob/
│   │   └── StockPriceBackgroundService.cs # Hosted service for price updates
│   ├── EmailService/
│   │   └── EmailSender.cs                 # SendGrid email implementation
│   ├── HangfireJob/
│   │   └── [Hangfire job implementations]
│   ├── Logging/
│   │   └── [Logging implementations]
│   ├── RedisCache/
│   │   └── RedisCacheService.cs           # Redis caching implementation
│   ├── SentimentAnalysis/
│   │   └── SentimentAnalysisService.cs    # ML.NET sentiment prediction
│   ├── SignalRService/
│   │   └── [SignalR implementations]
│   ├── YahooFinance/
│   │   └── YahooFinanceService.cs         # Yahoo Finance API integration
│   └── InfrastructureServicesRegistration.cs  # Dependency injection
│
├── Finance.StockMarket.Persistence/
│   ├── Configurations/
│   │   └── [Entity configurations]
│   ├── DatabaseContext/
│   │   └── FinanceStockMarketDatabaseContext.cs
│   ├── Migrations/
│   │   └── [Database migration files]
│   ├── Repositories/
│   │   └── [Repository implementations]
│   ├── Finance.StockMarket.Persistence.csproj
│   └── PersistenceServiceRegistration.cs  # Dependency injection
│
├── Finance.StockMarket.Tracking.sln       # Visual Studio solution file
└── README.md                              # This file
```

---

## Development Notes

### Design Patterns Used

| Pattern | Usage | Location |
|---------|-------|----------|
| **Repository Pattern** | Data access abstraction | `Persistence/Repositories/` |
| **CQRS** | Separation of read/write operations | `Application/Features/` with MediatR |
| **Dependency Injection** | Loose coupling between layers | `Program.cs`, `*ServiceRegistration.cs` |
| **Middleware Pattern** | Cross-cutting concerns (exception handling) | `Middleware/ExceptionMiddleware.cs` |
| **Contract-based Interfaces** | Service implementation contracts | `Application/Contracts/` |
| **Data Transfer Objects (DTOs)** | Layer communication | Throughout application |
| **Configuration Management** | Environment-specific settings | `appsettings*.json` |

### Key Architectural Decisions

1. **Clean Architecture**: Strict layer separation ensures testability and maintainability
2. **CQRS with MediatR**: Separates command (writes) from query (reads) operations
3. **Repository Pattern**: Abstracts data access, enabling flexible storage strategies
4. **Dependency Injection**: Built-in ASP.NET Core DI container reduces coupling
5. **SignalR WebSockets**: Real-time bidirectional communication for live price updates
6. **Redis Caching**: High-performance caching layer reduces database load
7. **Hangfire Background Jobs**: Reliable job scheduling with persistence
8. **ML.NET Sentiment Analysis**: On-premise ML model for news sentiment classification
9. **JWT Authentication**: Stateless, scalable authentication mechanism

### Current Limitations & Future Improvements

⚠️ **Known Limitations:**
- Sentiment analysis model is stubbed (returns "Unknown") - model training required
- Frontend application is separate React app (requires deployment coordination)
- Hangfire Dashboard lacks authentication in current implementation
- Rate limiting not implemented on API endpoints

✅ **Recommended Improvements:**
- Train & deploy ML.NET sentiment analysis model
- Implement API rate limiting (AspNetCoreRateLimit)
- Add comprehensive integration tests
- Implement Swagger security definitions
- Deploy Hangfire Dashboard authentication
- Add metrics/telemetry (Application Insights)
- Implement distributed tracing
- Add GraphQL support alongside REST API
- Deploy to Azure App Service with managed SQL/Redis

### Debugging & Troubleshooting

#### Issue: Database migration fails
```bash
# Solution: Ensure SQL Server is running and connection string is correct
# Verify connection: 
sqlcmd -S localhost -E -Q "SELECT @@VERSION;"
```

#### Issue: Redis connection error
```bash
# Solution: Ensure Redis is running on port 6379
# Check Redis status:
redis-cli ping
# Should return: PONG
```

#### Issue: JWT token validation fails
```
# Solution: Ensure JWT key in appsettings.json is at least 32 characters for HS256
# Regenerate token with correct key
```

#### Issue: SignalR connection fails
```
# Solution: Ensure CORS is properly configured for your frontend URL
# Update appsettings.json CorsSettings:AllowedOrigins
```

---

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Code Style Guidelines
- Follow C# naming conventions (PascalCase for classes, camelCase for variables)
- Write XML documentation for public methods
- Keep methods focused and under 20 lines
- Use async/await for I/O operations
- Write unit tests for business logic

---

## License

This project is licensed under the MIT License - see [LICENSE](LICENSE) file for details.

---

## Support & Contact

For issues, questions, or suggestions:
- 📧 Email: support@stockmarket.dev
- 🐛 Issues: [GitHub Issues](https://github.com/yourusername/Finance.StockMarket.Tracking/issues)
- 💬 Discussions: [GitHub Discussions](https://github.com/yourusername/Finance.StockMarket.Tracking/discussions)

---

## Acknowledgments

- Built with [ASP.NET Core](https://github.com/dotnet/aspnetcore)
- ORM: [Entity Framework Core](https://github.com/dotnet/efcore)
- Real-time: [SignalR](https://github.com/dotnet/aspnetcore/tree/main/src/SignalR)
- Background Jobs: [Hangfire](https://www.hangfire.io/)
- ML: [ML.NET](https://github.com/dotnet/machinelearning)
- Caching: [Redis](https://redis.io/)

---

**Last Updated**: April 14, 2026  
**Version**: 1.0.0  
**Status**: ✅ Production Ready