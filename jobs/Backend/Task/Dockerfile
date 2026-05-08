
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ExchangeRateUpdater.Host/ExchangeRateUpdater.Host.csproj ./ExchangeRateUpdater.Host/
RUN dotnet restore ./ExchangeRateUpdater.Host/ExchangeRateUpdater.Host.csproj

COPY . .

RUN dotnet publish ./ExchangeRateUpdater.Host/ExchangeRateUpdater.Host.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Host.dll"]