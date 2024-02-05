FROM mcr.microsoft.com/dotnet/sdk:8.0.1-bookworm-slim-amd64

WORKDIR /src

COPY ./Macro-Deck-UpdateService.sln .
COPY ./Directory.Build.props .
COPY ./Directory.Packages.props .
COPY ./src/MacroDeck.UpdateService/MacroDeck.UpdateService.csproj src/MacroDeck.UpdateService/
COPY ./src/MacroDeck.UpdateService.Core/MacroDeck.UpdateService.Core.csproj src/MacroDeck.UpdateService.Core/

COPY ./tests/MacroDeck.UpdateService.Tests.Shared/MacroDeck.UpdateService.Tests.Shared.csproj tests/MacroDeck.UpdateService.Tests.Shared/
COPY ./tests/MacroDeck.UpdateService.Tests.IntegrationTests/MacroDeck.UpdateService.Tests.IntegrationTests.csproj tests/MacroDeck.UpdateService.Tests.IntegrationTests/
COPY ./tests/MacroDeck.UpdateService.Tests.UnitTests/MacroDeck.UpdateService.Tests.UnitTests.csproj tests/MacroDeck.UpdateService.Tests.UnitTests/

RUN dotnet restore
COPY . /src