FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files to leverage Docker layer caching during restore
COPY ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj ExchangeRateUpdater.Api/
COPY ExchangeRateUpdater.Application/ExchangeRateUpdater.Application.csproj ExchangeRateUpdater.Application/
COPY ExchangeRateUpdater.Infrastructure/ExchangeRateUpdater.Infrastructure.csproj ExchangeRateUpdater.Infrastructure/
COPY ExchangeRateUpdater.Domain/ExchangeRateUpdater.Domain.csproj ExchangeRateUpdater.Domain/

RUN dotnet restore ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj

# Copy remaining sources
COPY . .

WORKDIR /src/ExchangeRateUpdater.Api
RUN dotnet build "ExchangeRateUpdater.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/ExchangeRateUpdater.Api
RUN dotnet publish "ExchangeRateUpdater.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./

# Start the published API DLL directly.
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Api.dll"]