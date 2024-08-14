
dotnet new webapi -n BlogBackend


mkdir -p Core/Entities Core/Interfaces Core/Services Core/Specifications \
Infrastructure/Data Infrastructure/Repositories Infrastructure/Migrations \
API/Controllers API/DTOs API/Filters API/MappingProfiles API/Middlewares API/Validators \
Shared/Helpers Shared/Constants Shared/Extensions Shared/Settings \
Tests/UnitTests Tests/IntegrationTests Configurations Docs

dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design


dotnet new tool-manifest

dotnet tool install --local dotnet-ef

dotnet add package xunit
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Newtonsoft.Json



dotnet ef migrations add InitialCreate --output-dir Infrastructure/Migrations
dotnet ef database update


dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package System.IdentityModel.Tokens.Jwt
