FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ExchangeRateUpdater.Console/ExchangeRateUpdater.Console.csproj", "src/ExchangeRateUpdater.Console/"]
COPY ["CNB.ApiClient/CNB.ApiClient.csproj", "src/CNB.ApiClient/"]
COPY ["ExchangeRateUpdater.Domain/ExchangeRateUpdater.Domain.csproj", "src/ExchangeRateUpdater.Domain/"]
COPY ["ExchangeRateUpdater.Shared/ExchangeRateUpdater.Shared.csproj", "src/ExchangeRateUpdater.Shared/"]
RUN dotnet restore "src/ExchangeRateUpdater.Console/ExchangeRateUpdater.Console.csproj"
COPY . .

RUN dotnet build "ExchangeRateUpdater.Console/ExchangeRateUpdater.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRateUpdater.Console/ExchangeRateUpdater.Console.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Console.dll"]
