using AutoMapper;
using MacroDeck.UpdateService.Core.DataAccess.Entities;
using MacroDeck.UpdateService.Core.DataTypes;

namespace MacroDeck.UpdateService.AutoMapper;

public class VersionProfile : Profile
{
    public VersionProfile()
    {
        CreateMap<VersionEntity, VersionInfo>()
            .ForMember(dest => dest.Downloads, opt => opt.Ignore())
            .ForMember(dest => dest.SupportedPlatforms,
                opt => opt.MapFrom(x => x.Files.Select(f => f.PlatformIdentifier)));
    }
}