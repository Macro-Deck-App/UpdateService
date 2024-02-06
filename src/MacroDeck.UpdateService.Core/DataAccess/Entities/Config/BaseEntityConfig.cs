using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MacroDeck.UpdateService.Core.DataAccess.Entities.Config;

public abstract class BaseEntityConfig<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    public required string TableName { get; init; }
    
    public required string ColumnPrefix { get; init; }
    
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName(ColumnPrefix + "id");

        builder.Property(x => x.CreatedTimestamp)
            .HasColumnName(ColumnPrefix + "created_timestamp")
            .IsRequired();
    }
}