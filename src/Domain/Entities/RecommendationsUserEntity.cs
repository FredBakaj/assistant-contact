namespace AssistantContract.Domain.Entities;

public class RecommendationsUserEntity: BaseAuditableEntity<int>
{
    public int ContactId { get; set; }

    public ContactEntity Contact { get; set; } = null!;
}
