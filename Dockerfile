
# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the solution file and project files
COPY blog.sln ./
COPY src/Api/Api.csproj ./src/Api/

# Restore dependencies
RUN dotnet restore

# Copy the entire source code
COPY . .

# Build the application
RUN dotnet publish src/Api/Api.csproj -c Release -o /out

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /out .

# Expose the port your app will run on
EXPOSE 80

# Set the entrypoint to run your application
ENTRYPOINT ["dotnet", "Api.dll"]

