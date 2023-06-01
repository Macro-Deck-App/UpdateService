namespace MacroDeck.UpdateService.Core.ErrorHandling;

public static class ErrorMessages
{
    public const string Generic = "Something went wrong";
    public const string NoVersionFound = "No version for this platform was found";
    public const string VersionDoesNotExist = "This version does not exist";
    public const string FileAlreadyExists = "A file for this version and platform already exists";
    public const string CannotParseVersionString = "Cannot parse version string. Format must be X.X.X or X.X.X-previewX";
    public const string NoFileUploaded = "No file was uploaded";
}