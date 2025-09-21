FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ./ExchangeRateUpdater.sln .
COPY ./Exchange.Api/ ./Exchange.Api/
COPY ./Exchange.Application/ ./Exchange.Application/
COPY ./Exchange.ConsoleApp/ ./Exchange.ConsoleApp/
COPY ./Exchange.Domain/ ./Exchange.Domain/
COPY ./Exchange.Infrastructure/ ./Exchange.Infrastructure/

COPY ./Exchange.Application.UnitTests ./Exchange.Application.UnitTests
COPY ./Exchange.Domain.UnitTests ./Exchange.Domain.UnitTests
COPY ./Exchange.Infrastructure.UnitTests ./Exchange.Infrastructure.UnitTests

RUN dotnet restore ExchangeRateUpdater.sln

COPY . .

# Build API
WORKDIR /src/Exchange.Api
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Build ConsoleApp
WORKDIR /src/Exchange.ConsoleApp
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

WORKDIR /src/Exchange.Api
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

WORKDIR /src/Exchange.ConsoleApp
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD [ "dotnet", "Exchange.ConsoleApp.dll" ]