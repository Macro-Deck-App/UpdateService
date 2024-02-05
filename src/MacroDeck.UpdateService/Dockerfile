ARG BASE_IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:8.0.1-bookworm-slim-amd64 AS base
WORKDIR /app

FROM ${BASE_IMAGE} AS build
WORKDIR /src/src/MacroDeck.UpdateService/

RUN dotnet msbuild "MacroDeck.UpdateService.csproj" -p:Configuration=Release

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

RUN rm -f Config/config.json

ENTRYPOINT ["dotnet", "MacroDeck.UpdateService.dll" ]