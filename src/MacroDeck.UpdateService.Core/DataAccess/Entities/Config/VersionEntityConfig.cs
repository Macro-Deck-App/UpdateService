using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities.Config;

public class VersionEntityConfig : BaseEntityConfig<VersionEntity>
{
    public VersionEntityConfig()
    {
        TableName = "version";
        ColumnPrefix = "v_";
    }

    public override void Configure(EntityTypeBuilder<VersionEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable(TableName);

        builder.HasIndex(x => x.Version)
            .IsUnique();

        builder.Property(x => x.Version)
            .HasColumnName(ColumnPrefix + "version_string")
            .IsRequired();

        builder.Property(x => x.Major)
            .HasColumnName(ColumnPrefix + "version_major")
            .IsRequired();

        builder.Property(x => x.Minor)
            .HasColumnName(ColumnPrefix + "version_minor")
            .IsRequired();

        builder.Property(x => x.Patch)
            .HasColumnName(ColumnPrefix + "version_patch")
            .IsRequired();

        builder.Property(x => x.PreReleaseNo)
            .HasColumnName(ColumnPrefix + "version_pre_release_no");

        builder.Property(x => x.IsBetaVersion)
            .HasColumnName(ColumnPrefix + "is_pre_release_version")
            .HasDefaultValue(false);

        builder.HasMany(x => x.Files)
            .WithOne(x => x.Version)
            .HasForeignKey(x => x.VersionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}