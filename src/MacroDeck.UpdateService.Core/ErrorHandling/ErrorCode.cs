namespace MacroDeck.UpdateService.Core.ErrorHandling;

public enum ErrorCode
{
    // Generic (-100xxxx)
    InternalError = -1001000,
    
    // Versions (-200xxxx)
    NoVersionFound = -2001000,
    VersionDoesNotExist = -20020000,
    CannotParseVersionString = -2003000,
    
    // Files (-300xxxx)
    FileAlreadyExists = -3001000,
    FileNotUploaded = -3002000,
}