#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ExchangeRatesGetterWorkerService/ExchangeRatesGetterWorkerService.csproj", "ExchangeRatesGetterWorkerService/"]
RUN dotnet restore "ExchangeRatesGetterWorkerService/ExchangeRatesGetterWorkerService.csproj"
COPY . .
WORKDIR "/src/ExchangeRatesGetterWorkerService"
RUN dotnet build "ExchangeRatesGetterWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRatesGetterWorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRatesGetterWorkerService.dll"]