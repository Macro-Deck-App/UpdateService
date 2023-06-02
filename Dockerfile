FROM mcr.microsoft.com/dotnet/sdk:7.0.302-bullseye-slim-amd64 AS restore
FROM mcr.microsoft.com/dotnet/aspnet:7.0.5-bullseye-slim-amd64 AS base

WORKDIR /src

COPY ./Macro-Deck-UpdateService.sln .
COPY ./Directory.Build.props .
COPY ./Directory.Packages.props .
COPY ./src/MacroDeck.UpdateService .
COPY ./src/MacroDeck.UpdateService.Core .

COPY ./tests/MacroDeck.UpdateService.Tests.Shared .
COPY ./tests/MacroDeck.UpdateService.Tests.IntegrationTests .
COPY ./tests/MacroDeck.UpdateService.Tests.UnitTests .

RUN dotnet restore /src
COPY . /src

FROM restore AS build