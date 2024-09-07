# Blog Backend API

This is a professional-grade blog backend API built with .NET 8, implementing best practices in clean architecture, filtering, sorting, and pagination using the Sieve library.

## Features

- **Clean Architecture**: Follows a well-structured architecture with separate layers for Controllers, Services, Repositories, DTOs, and more.
- **Filtering and Sorting**: Advanced filtering and sorting using the Sieve library.
- **Pagination**: Supports pagination with metadata like total count, current page, and next/previous page indicators.
- **Entity Framework Core**: Uses EF Core for database operations.
- **AutoMapper**: Efficiently maps between entities and DTOs.
- **Swagger**: Integrated Swagger UI for API documentation and testing.

## Project Structure

```
├── README.md
├── blog.sln
├── src
│   └── Api
│       ├── API
│       │   ├── Behaviors
│       │   │   └── ValidationBehavior.cs
│       │   ├── Controllers
│       │   │   ├── AuthController.cs
│       │   │   └── PostsController.cs
│       │   ├── DTOs
│       │   │   ├── CreatePostDto.cs
│       │   │   ├── LoginRequestDTO.cs
│       │   │   ├── LoginResponseDTO.cs
│       │   │   ├── PagedResult.cs
:│       │   │   ├── PostDto.cs
│       │   │   ├── PostFilterDTO.cs
│       │   │   ├── PostFilterDto.cs
│       │   │   ├── RefreshTokenRequestDTO.cs
│       │   │   ├── RegisterRequestDTO.cs
│       │   │   └── UserFilterDTO.cs
│       │   ├── Exceptions
│       │   │   ├── ApiException.cs
│       │   │   └── CustomExceptions.cs
│       │   ├── Filters
│       │   │   ├── NumberFilter.cs
│       │   │   └── StringFilter.cs
│       │   ├── MappingProfiles
│       │   │   └── PostProfile.cs
│       │   ├── Middlewares
│       │   │   └── ExceptionHandlingMiddleware.cs
│       │   ├── Models
│       │   │   ├── AuthRequest.cs
│       │   │   ├── RefreshTokenRequest.cs
│       │   │   └── RegisterRequest.cs
│       │   ├── Responses
│       │   │   └── ApiResponse.cs
│       │   └── Validators
│       │       ├── AddPostValidator.cs
│       │       ├── PostValidator.cs
│       │       └── RegisterValidator.cs
│       ├── Api.csproj
│       ├── Core
│       │   ├── Entities
│       │   │   ├── Author.cs
│       │   │   ├── Category.cs
│       │   │   ├── Post.cs
│       │   │   └── User.cs
│       │   └── Interfaces
│       │       ├── IPostService.cs
│       │       ├── IRepository.cs
│       │       └── IUserRepository.cs
│       ├── Infrastructure
│       │   ├── Data
│       │   │   └── BlogDbContext.cs
│       │   ├── Migrations
│       │   │   ├── 20240820072758_InitialCreate.Designer.cs
│       │   │   ├── 20240820072758_InitialCreate.cs
│       │   │   └── BlogDbContextModelSnapshot.cs
│       │   ├── Repositories
│       │   │   ├── Repository.cs
│       │   │   └── UserRepository.cs
│       │   └── Services
│       │       ├── PostService.cs
│       │       └── TokenService.cs
│       ├── Program.cs
│       ├── Properties
│       │   └── launchSettings.json
│       ├── appsettings.Development.json
│       ├── appsettings.json
│       └── blog.db
└── tests
    └── Api.IntegrationTests
        ├── Api.IntegrationTests.csproj
        ├── GlobalUsings.cs
        ├── IntegrationTests
        │   └── Controllers
        │       ├── AuthControllerTests.cs
        │       ├── CustomWebApplicationFactory.cs
        │       ├── PostControllerTests.cs
        │       └── PostsControllerIntegrationTests.cs
        └── UnitTest1.cs
```


## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite](https://www.sqlite.org/index.html)

### Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/MohamedHamed12/Blog_dotnet
   cd blog-backend
2. Install the required packages:

   ```bash
      dotnet restore
   ```

3. Apply migrations and update the database:

   ```bash
   dotnet ef database update
   ```

4. Run the application:

   ```bash
   dotnet run
   ```



### API Documentation
- Swagger is integrated for easy API exploration and testing. Once the application is running, navigate to https://localhost:8000/swagger to access the API documentation.

### Example Requests
- Get All Posts with Filtering, Sorting, and Pagination:

```GET /api/posts?Sorts=title&Filters=title~test,createdAt>=2023-01-01&page=1&pageSize=10```

###  Tests
To run tests (assuming you have a test project set up):

```bash
dotnet test
```
### Contributing
Contributions are welcome! Please fork the repository and create a pull request with your changes.


