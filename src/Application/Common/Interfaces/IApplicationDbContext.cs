using AssistantContract.Domain.Entities;

namespace AssistantContract.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<UserEntity> Users { get; }
    public DbSet<StateTreeEntity> StateTree { get; }
    public DbSet<AdminNotificationEntity> AdminNotification { get; }
    public DbSet<ContactEntity> Contact { get; }
    public DbSet<LogEntity> Log { get; }
    public DbSet<RecommendationsUserEntity> RecommendationsUser { get; }
    public DbSet<UserNotificationEntity> UserNotification { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
