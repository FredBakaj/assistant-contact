namespace AssistantContract.Domain.Entities;

public class UserEventEntity: BaseAuditableEntity<int>
{
    public long? UserId { get; set; }
    public string EventName { get; set; } = null!;
    public string Data { get; set; } = null!;
}
