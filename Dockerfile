 Use the official .NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app
# Use the official .NET Core SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src/Api
COPY ["Api.csproj", "./"]
RUN dotnet restore "./Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish
# Build the final image using the base image and the published output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]