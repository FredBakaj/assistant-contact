using System.Reflection;
using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssistantContract.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<StateTreeEntity> StateTree => Set<StateTreeEntity>();
    public DbSet<AdminNotificationEntity> AdminNotification => Set<AdminNotificationEntity>();
    public DbSet<ContactEntity> Contact => Set<ContactEntity>();
    public DbSet<LogEntity> Log => Set<LogEntity>();
    public DbSet<RecommendationsUserEntity> RecommendationsUser => Set<RecommendationsUserEntity>();
    public DbSet<UserNotificationEntity> UserNotification => Set<UserNotificationEntity>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
