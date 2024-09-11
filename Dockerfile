
# # Stage 1: Build the application
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# # Set the working directory in the container
# WORKDIR /app

# # Copy the solution file
# COPY blog.sln ./

# # Copy the project files
# COPY src/Api/Api.csproj src/Api/
# COPY tests/Api.IntegrationTests/Api.IntegrationTests.csproj tests/Api.IntegrationTests/

# # Restore dependencies for the entire solution
# RUN dotnet restore

# # Copy the entire source code
# COPY src ./src
# COPY tests ./tests

# # Build the application
# RUN dotnet publish src/Api/Api.csproj -c Release -o /out

# # Stage 2: Build a runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# WORKDIR /app

# # Copy the published output from the build stage
# COPY --from=build /out .

# # Expose the port your app will run on
# EXPOSE 80

# # Set the entrypoint to run your application
# ENTRYPOINT ["dotnet", "Api.dll"]



# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG TARGETARCH
# WORKDIR /source

# # copy csproj and restore as distinct layers
# COPY src/Api/*.csproj .
# RUN dotnet restore -a $TARGETARCH

# # copy and publish app and libraries
# COPY aspnetapp/. .
# RUN dotnet publish -a $TARGETARCH --no-restore -o /app


# # final stage/image
# FROM mcr.microsoft.com/dotnet/aspnet:8.0
# EXPOSE 8080
# WORKDIR /app
# COPY --from=build /app .
# USER $APP_UID
# ENTRYPOINT ["./aspnetapp"]


# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

# Build stage
# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG TARGETARCH
# WORKDIR /source

# # Copy csproj and restore as distinct layers
# COPY src/Api/*.csproj ./Api/
# RUN dotnet restore ./Api/Api.csproj

# # Copy and publish app and libraries
# COPY src/Api/. ./Api/
# RUN dotnet publish ./Api/Api.csproj -c Release -a $TARGETARCH --no-restore -o /app

# # Final stage/image
# FROM mcr.microsoft.com/dotnet/aspnet:8.0
# EXPOSE 8080
# WORKDIR /app
# COPY --from=build /app .
# ENTRYPOINT ["dotnet", "Api.dll"]






# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
ARG TARGETARCH
WORKDIR /source

# copy csproj and restore as distinct layers
COPY src/Api/*.csproj ./Api/

RUN dotnet restore -a $TARGETARCH

# copy everything else and build app
COPY src/Api/. ./Api/

RUN dotnet publish -a $TARGETARCH --no-restore -o /app


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["dotnet", "Api.dll"]












FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
RUN dotnet restore "./Api/Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]