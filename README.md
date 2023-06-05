# Macro Deck Update Service API V2 Version Controller Documentation

The `ApiV2VersionController` in the Macro Deck Update Service provides methods for managing and interacting with Macro Deck versions. This controller is part of the API V2 and includes methods for checking for updates, retrieving the latest version, getting information about a specific version, and getting the file size of a specific version for a specific platform.

## Endpoints

### GET /v2versions/check/{installedVersion}/{platform}

This endpoint checks for newer versions of Macro Deck based on the currently installed version and the platform.

- **Parameters**:
  - `apiVersion`: The API version.
  - `installedVersion`: A string representation of the installed version.
  - `platform`: A `PlatformIdentifier` enum representing the Macro Deck's platform.
  - `previewVersions` (optional, query): A boolean indicating whether to include preview versions in the check.
- **Returns**: An object representing the `ApiV2CheckResult`.

### GET /v2/versions/latest/{platform}

This endpoint returns the latest version of Macro Deck for a specific platform.

- **Parameters**:
  - `apiVersion`: The API version.
  - `platform`: A `PlatformIdentifier` enum representing the Macro Deck's platform.
  - `previewVersions` (optional, query): A boolean indicating whether to include preview versions in the response.
- **Returns**: An object representing the `ApiV2VersionInfo`.

### GET /v2/versions/{version}

This endpoint returns information about a specific version of Macro Deck.

- **Parameters**:
  - `apiVersion`: The API version.
  - `version`: A string representation of the version.
- **Returns**: An object representing the `ApiV2VersionInfo`.

### GET /v2/versions/{version}/fileSize/{platform}

This endpoint returns the file size of a specific version of Macro Deck for a specific platform.

- **Parameters**:
  - `apiVersion`: The API version.
  - `version`: A string representation of the version.
  - `platform`: A `PlatformIdentifier` enum representing the Macro Deck's platform.
- **Returns**: The file size as a double.

## Data Types

### `ApiV2VersionInfo`

The `ApiV2VersionInfo` class provides information about a Macro Deck version. This includes the version number and a dictionary of platform identifiers each mapped to their respective download link.

- **Properties**:
  - `Version`: A string representation of the Macro Deck version.
  - `Platforms`: A dictionary mapping `PlatformIdentifier` enum values to their respective download links for the specified version.

### `ApiV2CheckResult`

The `ApiV2CheckResult` class provides the result of a version check for Macro Deck. It includes a boolean indicating if a newer version is available and an `ApiV2VersionInfo` object providing details about the available version, if applicable.

- **Properties**:
  - `NewerVersionAvailable`: A boolean indicating whether a newer version of Macro Deck is available.
  - `Version`: An instance of `ApiV2VersionInfo` providing information about the newer version if one is available; null otherwise.
