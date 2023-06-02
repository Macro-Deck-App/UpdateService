using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities.Config;

public class VersionFileEntityConfig : BaseEntityConfig<VersionFileEntity>
{
    public VersionFileEntityConfig()
    {
        TableName = "version_files";
    }

    public override void Configure(EntityTypeBuilder<VersionFileEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable(TableName, schema: Schema);

        builder.HasIndex(x => x.VersionId);

        builder.Property(x => x.PlatformIdentifier)
            .HasColumnName("platform_identifier")
            .IsRequired();

        builder.Property(x => x.OriginalFileName)
            .HasColumnName("file_name")
            .IsRequired();
        
        builder.Property(x => x.SavedFileName)
            .HasColumnName("saved_name")
            .IsRequired();
        
        builder.Property(x => x.FileHash)
            .HasColumnName("hash")
            .IsRequired();

        builder.Property(x => x.FileSize)
            .HasColumnName("file_size")
            .IsRequired();

        builder.Property(x => x.VersionId)
            .HasColumnName("version_ref")
            .IsRequired();

        builder.HasMany(x => x.FileDownloads)
            .WithOne(x => x.VersionFile)
            .HasForeignKey(x => x.VersionFileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}