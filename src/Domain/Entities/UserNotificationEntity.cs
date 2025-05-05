namespace AssistantContract.Domain.Entities;

public class UserNotificationEntity : BaseAuditableEntity<int>
{
    public long UserId { get; set; }
    public int ContactNumber { get; set; }
}
