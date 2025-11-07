FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /
COPY ./ ./

RUN dotnet restore "src/Mews.ExchangeRateUpdater.Api/Mews.ExchangeRateUpdater.Api.csproj"

WORKDIR /src/Mews.ExchangeRateUpdater.Api
RUN dotnet build Mews.ExchangeRateUpdater.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Mews.ExchangeRateUpdater.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mews.ExchangeRateUpdater.Api.dll"]