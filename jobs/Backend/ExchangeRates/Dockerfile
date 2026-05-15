#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ExchangeRates/ExchangeRates.csproj", "src/ExchangeRates/"]
COPY ["src/CNB.Client/CNB.Client.csproj", "src/CNB.Client/"]
COPY ["src/ExchangeRates.Domain/ExchangeRates.Domain.csproj", "src/ExchangeRates.Domain/"]
RUN dotnet restore "src/ExchangeRates/ExchangeRates.csproj"
COPY . .
WORKDIR "src/ExchangeRates"
RUN dotnet build "ExchangeRates.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRates.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRates.dll"]