FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .

COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
RUN dotnet restore "ExchangeRateUpdater.sln"

RUN dotnet build "src/ExchangeRateProvider/ExchangeRateUpdater.csproj" -c Release -o /app/build

RUN dotnet test "test/ExchangeRateUpdater.Unit.Tests/ExchangeRateUpdater.Unit.Tests.csproj"
RUN dotnet test "test/ExchangeRateUpdater.Integration.Tests/ExchangeRateUpdater.Integration.Tests.csproj"

FROM build AS publish
RUN dotnet publish "src/ExchangeRateProvider/ExchangeRateUpdater.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ExchangeRateUpdater.dll"]