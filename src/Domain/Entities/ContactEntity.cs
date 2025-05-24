namespace AssistantContract.Domain.Entities;

public class ContactEntity : BaseAuditableEntity<int>
{
    public int ContactNumber { get; set; }
    public string Name { get; set; } = null!;
    public string? PersonalInfo { get; set; }
    public string Description { get; set; } = null!;

    public string KeywordDescription { get; set; } = null!;
    public int NotificationDayTimeSpan { get; set; }

    public long UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public List<RecommendationsUserEntity> RecommendationsUser { get; set; } = null!;
}
