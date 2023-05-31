using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities.Config;

public class FileDownloadEntityConfig : BaseEntityConfig<FileDownloadEntity>
{
    public FileDownloadEntityConfig()
    {
        TableName = "file_downloads";
    }

    public override void Configure(EntityTypeBuilder<FileDownloadEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable(TableName, schema: Schema);

        builder.HasIndex(x => x.VersionFileId);
        builder.HasIndex(x => x.DownloadReason);
        builder.HasIndex(x => x.PlatformIdentifier);

        builder.Property(x => x.DownloadReason)
            .HasColumnName("download_reason")
            .IsRequired();
        
        builder.Property(x => x.PlatformIdentifier)
            .HasColumnName("platform")
            .IsRequired();

        builder.Property(x => x.VersionFileId)
            .HasColumnName("version_file_ref")
            .IsRequired();
    }
}