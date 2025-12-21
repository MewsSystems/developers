FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy Directory.Build.props first (contains TargetFramework)
COPY ["Directory.Build.props", "./"]

# Copy all project files for dependency resolution
COPY ["src/ExchangeRateProvider.ConsoleApp/ExchangeRateProvider.ConsoleApp.csproj", "src/ExchangeRateProvider.ConsoleApp/"]
COPY ["src/ExchangeRateProvider.Application/ExchangeRateProvider.Application.csproj", "src/ExchangeRateProvider.Application/"]
COPY ["src/ExchangeRateProvider.Domain/ExchangeRateProvider.Domain.csproj", "src/ExchangeRateProvider.Domain/"]
COPY ["src/ExchangeRateProvider.Infrastructure/ExchangeRateProvider.Infrastructure.csproj", "src/ExchangeRateProvider.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/ExchangeRateProvider.ConsoleApp/ExchangeRateProvider.ConsoleApp.csproj"

# Copy all source code
COPY . .

# Build the application
RUN dotnet build "src/ExchangeRateProvider.ConsoleApp/ExchangeRateProvider.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/ExchangeRateProvider.ConsoleApp/ExchangeRateProvider.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateProvider.ConsoleApp.dll"]