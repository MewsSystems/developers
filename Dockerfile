FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY jobs/Backend/Task/ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj ExchangeRateUpdater.Api/
COPY jobs/Backend/Task/ExchangeRateUpdater.Core/ExchangeRateUpdater.Core.csproj ExchangeRateUpdater.Core/
COPY jobs/Backend/Task/ExchangeRateUpdater.Application/ExchangeRateUpdater.Application.csproj ExchangeRateUpdater.Application/
COPY jobs/Backend/Task/ExchangeRateUpdater.Infrastructure/ExchangeRateUpdater.Infrastructure.csproj ExchangeRateUpdater.Infrastructure/
RUN dotnet restore ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj

# Copy everything else and build
COPY jobs/Backend/Task/ExchangeRateUpdater.Api/. ExchangeRateUpdater.Api/
COPY jobs/Backend/Task/ExchangeRateUpdater.Core/. ExchangeRateUpdater.Core/
COPY jobs/Backend/Task/ExchangeRateUpdater.Application/. ExchangeRateUpdater.Application/
COPY jobs/Backend/Task/ExchangeRateUpdater.Infrastructure/. ExchangeRateUpdater.Infrastructure/

RUN dotnet build ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj -c Debug -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj -c Debug -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Api.dll"] 