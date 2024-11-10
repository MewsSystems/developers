FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY Directory.Packages.props .
COPY Directory.Build.props . 
COPY "ExchangeRateUpdater.Infra/ExchangeRateUpdater.Infra.csproj" "ExchangeRateUpdater.Infra/ExchangeRateUpdater.Infra.csproj"
COPY "ExchangeRateUpdater.Core/ExchangeRateUpdater.Core.csproj" "ExchangeRateUpdater.Core/ExchangeRateUpdater.Core.csproj"
COPY "ExchangeRateUpdater.Console/ExchangeRateUpdater.Console.csproj" "ExchangeRateUpdater.Console/ExchangeRateUpdater.Console.csproj"

RUN dotnet restore "ExchangeRateUpdater.Console/ExchangeRateUpdater.Console.csproj"
COPY . ./
RUN dotnet publish "ExchangeRateUpdater.Console/ExchangeRateUpdater.Console.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Console.dll"]
