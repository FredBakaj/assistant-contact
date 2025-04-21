using AssistantContract.TgBot.Core.Src.Controller;

namespace AssistantContract.TgBot.Core.Factory.Implementation;

public class ControllerFactory : IFactory<IBotController>
{
    private readonly IEnumerable<IBotController> _strategies;

    public ControllerFactory(IEnumerable<IBotController> strategies)
    {
        _strategies = strategies;
    }

    public Task<IBotController> CreateAsync(object value)
    {
        string value_ = value as string ?? string.Empty;
        return Task.FromResult(_strategies.First(s => s.Name() == value_));
    }
}
