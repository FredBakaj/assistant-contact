using AssistantContract.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssistantContract.Infrastructure.Data.Configurations;

public class AdminNotificationConfig : BaseConfig, IEntityTypeConfiguration<AdminNotificationEntity>
{
    public void Configure(EntityTypeBuilder<AdminNotificationEntity> builder)
    {
        builder.ToTable("AdminNotification");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .HasColumnType("int");
        builder.HasQueryFilter(x => x.WhenDeleted == null);
    }
}
