namespace AssistantContract.Domain.Entities;

public class UserEntity : BaseAuditableEntity<long>
{
    public string Name { get; set; } = null!;
    public StateTreeEntity StateTree { get; set; } = null!;
    public List<AdminNotificationEntity> AdminNotification { get; set; } = null!;
    public List<ContactEntity> Contact { get; set; } = null!;
}
