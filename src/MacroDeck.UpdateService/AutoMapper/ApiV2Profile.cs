using AutoMapper;
using MacroDeck.UpdateService.Core.DataTypes;
using MacroDeck.UpdateService.Core.DataTypes.ApiV2;

namespace MacroDeck.UpdateService.AutoMapper;

public class ApiV2Profile : Profile
{
    public ApiV2Profile()
    {
        CreateMap<VersionInfo, ApiV2VersionInfo>();
        CreateMap<CheckResult, ApiV2CheckResult>();
    }
}