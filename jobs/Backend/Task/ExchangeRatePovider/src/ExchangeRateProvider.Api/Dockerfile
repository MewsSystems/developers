FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy Directory.Build.props first (contains TargetFramework)
COPY ["Directory.Build.props", "./"]

# Copy all project files for dependency resolution
COPY ["src/ExchangeRateProvider.Api/ExchangeRateProvider.Api.csproj", "src/ExchangeRateProvider.Api/"]
COPY ["src/ExchangeRateProvider.Application/ExchangeRateProvider.Application.csproj", "src/ExchangeRateProvider.Application/"]
COPY ["src/ExchangeRateProvider.Domain/ExchangeRateProvider.Domain.csproj", "src/ExchangeRateProvider.Domain/"]
COPY ["src/ExchangeRateProvider.Infrastructure/ExchangeRateProvider.Infrastructure.csproj", "src/ExchangeRateProvider.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/ExchangeRateProvider.Api/ExchangeRateProvider.Api.csproj"

# Copy all source code
COPY . .

# Build the application
RUN dotnet build "src/ExchangeRateProvider.Api/ExchangeRateProvider.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/ExchangeRateProvider.Api/ExchangeRateProvider.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateProvider.Api.dll"]