using AssistantContract.TgBot.Core.Model;

namespace AssistantContract.TgBot.Core.ViewDto;

public abstract class BaseVDto
{
    public UpdateBDto Update { get; set; } = null!;
}
