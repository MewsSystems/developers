FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ExchangeRateUpdater.API/ExchangeRateUpdater.API.csproj", "ExchangeRateUpdater.API/"]
COPY ["src/ExchangeRateUpdater.Application/ExchangeRateUpdater.Application.csproj", "ExchangeRateUpdater.Application/"]
RUN dotnet restore "ExchangeRateUpdater.API/ExchangeRateUpdater.API.csproj"
COPY /src .
WORKDIR "/src/ExchangeRateUpdater.API"
RUN dotnet build "ExchangeRateUpdater.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRateUpdater.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --chown=dotnet:dotnet --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateUpdater.API.dll"]
