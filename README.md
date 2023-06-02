# MacroDeck.UpdateService API v2

This API provides endpoints to check for software updates, fetch version details, and download specific versions for different platforms.

## **Endpoint: Check for Updates**

`GET /v2/check/{installedVersion}/{platform}`

Checks if there is a newer version available than the installed one.

### Parameters

- `installedVersion`: The current installed version of the software.
- `platform`: The platform identifier where the software is running (See PlatformIdentifier enum values).

### Query Parameters

- `previewVersions`: Boolean value that indicates if preview versions should be considered. Defaults to `false`.

### Responses

Returns an `ApiV2CheckResult` object with properties:

- `NewerVersionAvailable`: Boolean indicating if a newer version is available.
- `Version`: String representing the newer version, if available.

---

## **Endpoint: Get Latest Version**

`GET /v2/latest/{platform}`

Fetches the latest software version details for the specified platform.

### Parameters

- `platform`: The platform identifier (See PlatformIdentifier enum values).

### Query Parameters

- `previewVersions`: Boolean value that indicates if preview versions should be considered. Defaults to `false`.

### Responses

Returns an `ApiV2VersionInfo` object with properties:

- `Version`: String representing the version number.
- `VersionState`: State of the version.
- `Downloads`: Long representing the download count.
- `SupportedPlatforms`: Array of supported platforms.

---

## **Endpoint: Get Version**

`GET /v2/{version}`

Fetches the specific software version details.

### Parameters

- `version`: The version of the software to retrieve details for.

### Responses

Returns an `ApiV2VersionInfo` object similar to the Get Latest Version endpoint.

---

## **Endpoint: Download Version**

`GET /v2/{version}/download/{platform}`

Downloads the specific version for the specified platform.

### Parameters

- `version`: The version of the software to download.
- `platform`: The platform identifier for the download.

### Query Parameters

- `downloadReason`: Indicates the reason for download (See DownloadReason enum values). Defaults to `FirstDownload`.

### Responses

Returns a byte array representing the file contents of the version. A header "x-file-hash" with the file's hash is also set in the response.

---

## Enums

### **PlatformIdentifier**

- `WinX64`: Windows 64-bit
- `MacX64`: Mac OS 64-bit
- `MacArm64`: Mac OS Arm 64-bit
- `LinuxX64`: Linux 64-bit
- `LinuxArm64`: Linux Arm 64-bit
- `LinuxArm32`: Linux Arm 32-bit

### **DownloadReason**

- `FirstDownload`: The version is being downloaded for the first time.
- `UpdateDownload`: The version is being downloaded as an update to a previous version.
