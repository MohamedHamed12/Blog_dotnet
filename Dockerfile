








# # FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# # USER app
# # WORKDIR /app
# # EXPOSE 8080
# # EXPOSE 8081

# # FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# # ARG BUILD_CONFIGURATION=Release
# # WORKDIR /src
# # COPY ["Api/Api.csproj", "Api/"]
# # RUN dotnet restore "./Api/Api.csproj"
# # COPY . .
# # WORKDIR "/src/Api"
# # RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# # FROM build AS publish
# # ARG BUILD_CONFIGURATION=Release
# # RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# # FROM base AS final
# # WORKDIR /app
# # COPY --from=publish /app/publish .
# # ENTRYPOINT ["dotnet", "Api.dll"]


# # Base image for running the app
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 8081

# # Build image for building the app
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src

# # Copy the solution and project files
# COPY blog.sln ./
# COPY src/Api/Api.csproj src/Api/

# # Restore dependencies
# RUN dotnet restore src/Api/Api.csproj

# # Copy the rest of the code
# COPY src/ src/

# # Build the app
# WORKDIR /src/Api
# RUN dotnet build Api.csproj -c Release -o /app/build

# # Publish the app
# FROM build AS publish
# RUN dotnet publish Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# # Final stage/image
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "Api.dll"]
# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and the project file(s)
COPY blog.sln ./
COPY src/Api/Api.csproj src/Api/

# Restore dependencies
RUN dotnet restore src/Api/Api.csproj

# Copy the rest of the source code
COPY src/ src/

# Build the app
WORKDIR /src/Api
RUN dotnet build Api.csproj -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
