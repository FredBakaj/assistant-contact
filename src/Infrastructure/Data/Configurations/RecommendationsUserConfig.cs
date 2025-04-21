using AssistantContract.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssistantContract.Infrastructure.Data.Configurations;

public class RecommendationsUserConfig: BaseConfig, IEntityTypeConfiguration<RecommendationsUserEntity>
{
    public void Configure(EntityTypeBuilder<RecommendationsUserEntity> builder)
    {
        builder.ToTable("RecommendationsUser");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn(1, 1)
            .HasColumnType("int");
        builder.HasQueryFilter(x => x.WhenDeleted == null);

        builder.HasOne(x => x.Contact)
            .WithMany(x => x.RecommendationsUser)
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
