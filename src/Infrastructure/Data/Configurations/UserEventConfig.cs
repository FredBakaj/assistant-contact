using AssistantContract.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssistantContract.Infrastructure.Data.Configurations;

public class UserEventConfig: BaseConfig, IEntityTypeConfiguration<UserEventEntity>
{
    public void Configure(EntityTypeBuilder<UserEventEntity> builder)
    {
        builder.ToTable("UserEvent");
        builder.HasKey(x => x.Id);
        builder.HasQueryFilter(x => x.WhenDeleted == null);
        
    }
}
