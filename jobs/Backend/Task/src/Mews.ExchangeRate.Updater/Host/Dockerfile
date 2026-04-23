#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["cnb/cnb.csproj", "cnb/"]
RUN dotnet restore "cnb/cnb.csproj"
COPY . .
WORKDIR "/src/cnb"
RUN dotnet build "cnb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cnb.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cnb.dll"]