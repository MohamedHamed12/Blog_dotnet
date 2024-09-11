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

# # Copy the rest of the source code
# COPY src/ src/

# # Debugging: List files in /src to see where they are copied
# RUN ls -la /src
# RUN ls -la /src/Api

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

# Copy the solution file and project files
COPY blog.sln ./
COPY src/Api/Api.csproj src/Api/

# Restore dependencies
RUN dotnet restore src/Api/Api.csproj

# Copy the rest of the source code
COPY src/ src/

# Debugging: List files in /src to see where they are copied
RUN ls -la /src
RUN ls -la /src/Api

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
