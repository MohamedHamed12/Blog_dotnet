# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:
    # 8.0 AS build
# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS base

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS base

USER app
WORKDIR /app
EXPOSE 8080
# EXPOSE 8081
EXPOSE 80
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/Api.csproj", "Api/"]
RUN dotnet restore "Api/Api.csproj"
COPY . .
WORKDIR "/src/src/Api"
RUN dotnet build "Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# CMD ["sh", "-c", "dotnet ef database update --project /app/publish/Api.dll --startup-project /app/publish/Api.dll && dotnet Api.dll"]
# CMD ["sh", "-c", "dotnet ef database update --project /app/publish/Api.dll "]
#
# ENTRYPOINT ["dotnet", "Api.dll"]
#


CMD ["sh", "-c", "dotnet ef database update --project /app/publish/Api.dll && dotnet Api.dll"]



