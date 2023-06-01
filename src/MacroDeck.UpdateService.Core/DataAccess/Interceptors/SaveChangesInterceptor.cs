using MacroDeck.UpdateService.Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Version = MacroDeck.UpdateService.Core.DataTypes.Version;

namespace MacroDeck.UpdateService.Core.DataAccess.Interceptors;

public class SaveChangesInterceptor : ISaveChangesInterceptor
{
    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context?.ChangeTracker.Entries() is null)
        {
            return ValueTask.FromResult(result);
        }

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedTimestamp = DateTime.Now;
                        break;
                }
            }

            if (entry.Entity is VersionEntity versionEntity
                && entry.State is EntityState.Added or EntityState.Modified)
            {
                var versionStruct = Version.Parse(versionEntity.Version);
                versionEntity.Major = versionStruct.Major;
                versionEntity.Minor = versionStruct.Minor;
                versionEntity.Patch = versionStruct.Patch;
                versionEntity.PreviewNo = versionStruct.PreviewNo;
                versionEntity.IsPreviewVersion = versionStruct.IsPreviewVersion;
            }
        }

        return ValueTask.FromResult(result);
    }
}