namespace AssistantContract.TgBot.Core.Model.Data;

public class AddContactDataDto
{
    public string? Name { get; set; }
    public string? PersonalInfo { get; set; }
    public string? Description { get; set; }
    public int NotificationDayTimeSpan { get; set; }
}
