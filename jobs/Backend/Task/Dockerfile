FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG TARGETARCH

COPY . /source

WORKDIR /source/ExchangeRateUpdater.Api

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

COPY --from=build /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ExchangeRateUpdater.Api.dll"]