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

        builder.Property(x => x.FileProvider)
            .HasColumnName("file_provider")
            .IsRequired();
        
        builder.Property(x => x.FileName)
            .HasColumnName("file_name")
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
    }
}