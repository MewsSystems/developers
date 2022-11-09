FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ERU.Console/ERU.Console.csproj", "ERU.Console/"]
COPY ["ERU.Domain/ERU.Domain.csproj", "ERU.Domain/"]
COPY ["ERU.Application/ERU.Application.csproj", "ERU.Application/"]
RUN dotnet restore "ERU.Console/ERU.Console.csproj"
COPY . .
WORKDIR "/src/ERU.Console"
RUN dotnet build "ERU.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERU.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERU.Console.dll"]
