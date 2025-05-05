using AssistantContract.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssistantContract.Infrastructure.Data.Configurations;

public class UserNotificationConfig : BaseConfig, IEntityTypeConfiguration<UserNotificationEntity>
{
    public void Configure(EntityTypeBuilder<UserNotificationEntity> builder)
    {
        builder.ToTable("UserNotification");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
    }
}
