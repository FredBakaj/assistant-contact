namespace AssistantContract.TgBot.Core.Model.Data;

public class ChangeContactDataDto
{
    public string? Description { get; set; }
    public string? PersonalInfo { get; set; }
    public int? NotificationDayTimeSpan { get; set; }
    public int ContactNumber { get; set; }
}
