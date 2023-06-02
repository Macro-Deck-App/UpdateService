FROM mcr.microsoft.com/dotnet/sdk:7.0.302-bullseye-slim-amd64 AS restore

WORKDIR /src

COPY ./Macro-Deck-UpdateService.sln .
COPY ./Directory.Build.props .
COPY ./Directory.Packages.props .
COPY ./src/MacroDeck.UpdateService .
COPY ./src/MacroDeck.UpdateService.Core .

COPY ./tests/MacroDeck.UpdateService.Tests.Shared .
COPY ./tests/MacroDeck.UpdateService.Tests.IntegrationTests .
COPY ./tests/MacroDeck.UpdateService.Tests.UnitTests .

RUN dotnet restore ./Macro-Deck-UpdateService.sln
COPY . /src