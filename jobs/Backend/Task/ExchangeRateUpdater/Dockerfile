FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExchangeRateUpdater/ExchangeRateUpdater.csproj", "ExchangeRateUpdater/"]
COPY ["ExchangeRateUpdater.Domain/ExchangeRateUpdater.Domain.csproj", "ExchangeRateUpdater.Domain/"]
RUN dotnet restore "ExchangeRateUpdater/ExchangeRateUpdater.csproj"
COPY . .
WORKDIR "/src/ExchangeRateUpdater"
RUN dotnet build "ExchangeRateUpdater.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRateUpdater.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.dll"]
