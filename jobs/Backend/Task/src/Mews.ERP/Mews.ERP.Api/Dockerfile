FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Mews.ERP.Api/Mews.ERP.Api.csproj", "Mews.ERP.Api/"]
COPY ["Mews.ERP.Api.UnitTests/Mews.ERP.Api.UnitTests.csproj", "Mews.ERP.Api.UnitTests/"]
COPY ["Mews.ERP.AppService/Mews.ERP.AppService.csproj", "Mews.ERP.AppService/"]
COPY ["Mews.ERP.AppService.UnitTests/Mews.ERP.AppService.UnitTests.csproj", "Mews.ERP.AppService.UnitTests/"]

RUN dotnet restore "Mews.ERP.Api/Mews.ERP.Api.csproj"
RUN dotnet restore "Mews.ERP.AppService/Mews.ERP.AppService.csproj"

COPY . .
WORKDIR "/src/Mews.ERP.Api"

RUN dotnet build "Mews.ERP.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mews.ERP.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5050
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mews.ERP.Api.dll"]
