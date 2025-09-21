FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ./ExchangeRateUpdater.sln .
COPY ./Exchange.Api/ ./Exchange.Api/
COPY ./Exchange.Application/ ./Exchange.Application/
COPY ./Exchange.Domain/ ./Exchange.Domain/
COPY ./Exchange.Infrastructure/ ./Exchange.Infrastructure/

RUN dotnet restore "Exchange.Api/Exchange.Api.csproj"

COPY . .

WORKDIR /src/Exchange.Api

RUN dotnet build "./Exchange.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./Exchange.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Exchange.Api.dll"]
