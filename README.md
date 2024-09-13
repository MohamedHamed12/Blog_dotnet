# Blog Backend API

This is a professional-grade blog backend API built with .NET 8, implementing best practices in clean architecture, filtering, sorting, and pagination using the Sieve library.

## Features

- **Clean Architecture**: Follows a well-structured architecture with separate layers for Controllers, Services, Repositories, DTOs, and more.
- **Filtering and Sorting**: Advanced filtering and sorting using the Sieve library.
- **Pagination**: Supports pagination with metadata like total count, current page, and next/previous page indicators.
- **Entity Framework Core**: Uses EF Core for database operations.
- **AutoMapper**: Efficiently maps between entities and DTOs.
- **Swagger**: Integrated Swagger UI for API documentation and testing.
- **SQLite**: Lightweight database for development and testing.
- **PostgreSQL for Production**: Robust and scalable **PostgreSQL** database for production environments.
- **Docker**: Supports containerization for seamless development, testing, and deployment.
- **Azure Deployment**: Deployable to **Azure** for cloud hosting and scalability.
- **CI/CD Pipeline**: Integrated **GitHub Actions** for automated CI/CD with Docker and Azure.
## Project Structure

```
.
├── Dockerfile
├── README.md
├── blog.sln
├── old
├── src
│   └── Api
│       ├── API
│       │   ├── Behaviors
│       │   │   └── ValidationBehavior.cs
│       │   ├── Controllers
│       │   │   ├── Auth
│       │   │   │   └── AuthController.cs
│       │   │   ├── Posts
│       │   │   │   ├── CommentsController.cs
│       │   │   │   └── PostsController.cs
│       │   │   └── Users
│       │   ├── DTOs
│       │   │   ├── Auth
│       │   │   │   ├── LoginDto.cs
│       │   │   │   ├── RegisterDto.cs
│       │   │   │   └── TokenResponseDto.cs
│       │   │   ├── PagedResult.cs
│       │   │   ├── Posts
│       │   │   │   ├── CommentDto.cs
│       │   │   │   └── PostDto.cs
│       │   │   └── Users
│       │   │       └── UserDto.cs
│       │   ├── Exceptions
│       │   │   ├── ApiException.cs
│       │   │   └── CustomExceptions.cs
│       │   ├── Extensions
│       │   │   └── ServiceExtensions.cs
│       │   ├── Filters
│       │   │   ├── NumberFilter.cs
│       │   │   └── StringFilter.cs
│       │   ├── MappingProfiles
│       │   │   └── PostProfile.cs
│       │   ├── Middlewares
│       │   │   └── ExceptionHandlingMiddleware.cs
│       │   ├── Models
│       │   │   ├── AuthRequest.cs
│       │   │   ├── RefreshTokenRequest.cs
│       │   │   └── RegisterRequest.cs
│       │   ├── Responses
│       │   │   └── ApiResponse.cs
│       │   └── Validators
│       │       ├── LoginValidator.cs
│       │       ├── PostValidator.cs
│       │       └── RegisterValidator.cs
│       ├── Api.csproj
│       ├── Core
│       │   ├── Entities
│       │   │   ├── Posts
│       │   │   │   ├── Comment.cs
│       │   │   │   └── Post.cs
│       │   │   └── Users
│       │   │       └── IdentityUser.cs
│       │   └── Interfaces
│       │       ├── Auth
│       │       │   └── IAuthService.cs
│       │       ├── IRepository.cs
│       │       ├── Posts
│       │       │   ├── ICommentService.cs
│       │       │   └── IPostService.cs
│       │       └── Users
│       │           └── IUserRepository.cs
│       ├── Infrastructure
│       │   ├── Data
│       │   │   └── BlogDbContext.cs
│       │   ├── Migrations
│       │   │   ├── 20240907073400_InitialCreate.Designer.cs
│       │   │   ├── 20240907073400_InitialCreate.cs
│       │   │   ├── 20240907105720_AddDateOfBirthToApplicationUser.Designer.cs
│       │   │   ├── 20240907105720_AddDateOfBirthToApplicationUser.cs
│       │   │   └── BlogDbContextModelSnapshot.cs
│       │   ├── Repositories
│       │   │   ├── Repository.cs
│       │   │   └── UserRepository.cs
│       │   └── Services
│       │       ├── Auth
│       │       │   └── AuthService.cs
│       │       ├── Posts
│       │       │   ├── CommentService.cs
│       │       │   └── PostService.cs
│       │       ├── TokenService.cs
│       │       └── Users
│       ├── Program.cs
│       ├── Properties
│       │   └── launchSettings.json
│       ├── appsettings.Development.json
│       ├── appsettings.json
│       └── blog.db
└── tests
    └── Api.IntegrationTests
        ├── Api.IntegrationTests.csproj
        ├── GlobalUsings.cs
        ├── IntegrationTests
        │   └── Controllers
        │       ├── AuthControllerTests.cs
        │       ├── CustomWebApplicationFactory.cs
        │       ├── PostControllerTests.cs
        │       └── PostsControllerIntegrationTests.cs
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


