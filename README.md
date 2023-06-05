# MacroDeck Update Service API V2 Documentation

The `ApiV2Controller` in the MacroDeck Update Service provides methods for managing and interacting with software versions. This controller is part of the API V2 and includes methods for retrieving the next major, minor, or patch version, validating version names, checking for updates, and getting the latest versions.

## Endpoints

### GET /v2/next/major

This endpoint returns the next major version of the software.

- **Parameters**: None.
- **Returns**: An object representing the `Version`.

### GET /v2/next/minor

This endpoint returns the next minor version of the software.

- **Parameters**: None.
- **Returns**: An object representing the `Version`.

### GET /v2/next/patch

This endpoint returns the next patch version of the software.

- **Parameters**: None.
- **Returns**: An object representing the `Version`.

### GET /v2/next/major/beta

This endpoint returns the next major beta version of the software.

- **Parameters**: None.
- **Returns**: An object representing the `Version`.

### GET /v2/next/minor/beta

This endpoint returns the next minor beta version of the software.

- **Parameters**: None.
- **Returns**: An object representing the `Version`.

### GET /v2/validate/versionname/{version}

This endpoint validates a version name.

- **Parameters**:
    - `version`: A string representation of the version to be validated.
- **Returns**: A boolean indicating whether the version name is valid or not.

### GET /v2/check/{installedVersion}/{platform}

This endpoint checks for newer versions of the software based on the currently installed version and the platform.

- **Parameters**:
    - `installedVersion`: A string representation of the installed version.
    - `platform`: A `PlatformIdentifier` enum representing the software's platform.
    - `previewVersions` (optional, query): A boolean indicating whether to include preview versions in the check.
- **Returns**: An object representing the `ApiV2CheckResult`.

### GET /v2/latest/{platform}

This endpoint returns the latest version of the software for a specific platform.

- **Parameters**:
    - `platform`: A `PlatformIdentifier` enum representing the software's platform.
    - `previewVersions` (optional, query): A boolean indicating whether to include preview versions in the response.
- **Returns**: An object representing the `ApiV2VersionInfo`.

### GET /v2/{version}

This endpoint returns information about a specific version.

- **Parameters**:
    - `version`: A string representation of the version.
- **Returns**: An object representing the `ApiV2VersionInfo`.

### GET /v2/{version}/fileSize/{platform}

This endpoint returns the file size of a specific version for a specific platform.

- **Parameters**:
    - `version`: A string representation of the version.
    - `platform`: A `PlatformIdentifier` enum representing the software's platform.
- **Returns**: The file size as a double.

## Data Types

### `Version`

The `Version` struct represents a version of the software and includes fields for the major, minor, and patch versions, as well as an optional beta number. This struct also provides methods for parsing and validating version strings.

### `ApiV2VersionInfo`

The `ApiV2VersionInfo` class provides information about a software version, including its version string and a dictionary of platforms and their respective version strings.

### `ApiV2CheckResult`

The `ApiV2CheckResult` class provides the result of a check for
