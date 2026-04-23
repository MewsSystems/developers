#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ExchangeRateDemo.API/ExchangeRateDemo.API.csproj", "ExchangeRateDemo.API/"]
COPY ["ExchangeRateDemo.Application/ExchangeRateDemo.Application.csproj", "ExchangeRateDemo.Application/"]
COPY ["ExchangeRateDemo.Infrastructure/ExchangeRateDemo.Infrastructure.csproj", "ExchangeRateDemo.Infrastructure/"]

RUN dotnet restore "./ExchangeRateDemo.API/ExchangeRateDemo.API.csproj"
COPY . .
WORKDIR "/src/ExchangeRateDemo.API"
RUN dotnet build "./ExchangeRateDemo.API.csproj" -o /app/build

FROM build AS publish
RUN dotnet publish "./ExchangeRateDemo.API.csproj" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateDemo.API.dll"]