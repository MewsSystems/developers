FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/ExchangeRateUpdater.Presentation.Cli/ExchangeRateUpdater.Presentation.Cli.csproj", "Presentation/ExchangeRateUpdater.Presentation.Cli/"]
RUN dotnet restore "Presentation/ExchangeRateUpdater.Presentation.Cli/ExchangeRateUpdater.Presentation.Cli.csproj"
COPY . .
WORKDIR "/src/Presentation/ExchangeRateUpdater.Presentation.Cli"
RUN dotnet build "ExchangeRateUpdater.Presentation.Cli.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExchangeRateUpdater.Presentation.Cli.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Presentation.Cli.dll"]
