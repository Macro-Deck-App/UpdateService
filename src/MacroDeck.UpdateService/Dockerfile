ARG BASE_IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:7.0.5-bullseye-slim-amd64 AS base
WORKDIR /app

FROM ${BASE_IMAGE} AS build
WORKDIR /src/MacroDeck.UpdateService/

RUN dotnet msbuild "MacroDeck.UpdateService.csproj" -warnAsMessage:CS0618 -warnAsError -p:Configuration=Release

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app

ENTRYPOINT ["dotnet", "MacroDeck.UpdateService.dll" ]