using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities.Config;

public class VersionFileEntityConfig : BaseEntityConfig<VersionFileEntity>
{
    public VersionFileEntityConfig()
    {
        TableName = "version_file";
        ColumnPrefix = "f_";
    }

    public override void Configure(EntityTypeBuilder<VersionFileEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable(TableName);

        builder.HasIndex(x => x.VersionId);

        builder.Property(x => x.PlatformIdentifier)
            .HasColumnName(ColumnPrefix + "platform_identifier")
            .IsRequired();

        builder.Property(x => x.FileProvider)
            .HasColumnName(ColumnPrefix + "file_provider")
            .IsRequired();
        
        builder.Property(x => x.FileName)
            .HasColumnName(ColumnPrefix + "file_name")
            .IsRequired();
        
        builder.Property(x => x.FileHash)
            .HasColumnName(ColumnPrefix + "hash")
            .IsRequired();

        builder.Property(x => x.FileSize)
            .HasColumnName(ColumnPrefix + "file_size")
            .IsRequired();

        builder.Property(x => x.VersionId)
            .HasColumnName(ColumnPrefix + "version_ref")
            .IsRequired();
    }
}