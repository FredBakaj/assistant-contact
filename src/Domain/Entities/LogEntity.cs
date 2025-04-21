namespace AssistantContract.Domain.Entities;

public class LogEntity:BaseAuditableEntity<int>
{
    public long? UserId { get; set; }
    public int Type { get; set; }
    public string Data { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string StackTrace { get; set; } = null!;
}
