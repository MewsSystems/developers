FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj", "src/ExchangeRateUpdater.Api/"]
COPY ["src/ExchangeRateUpdater.Core/ExchangeRateUpdater.Core.csproj", "src/ExchangeRateUpdater.Core/"]
COPY ["src/ExchangeRateUpdater.Infrastructure/ExchangeRateUpdater.Infrastructure.csproj", "src/ExchangeRateUpdater.Infrastructure/"]
RUN dotnet restore "src/ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj"
COPY . .
RUN dotnet build "src/ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/ExchangeRateUpdater.Api/ExchangeRateUpdater.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Api.dll"]
