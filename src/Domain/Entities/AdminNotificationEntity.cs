namespace AssistantContract.Domain.Entities;

public class AdminNotificationEntity : BaseAuditableEntity<int>
{
    public string Message { get; set; } = null!;

    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}
