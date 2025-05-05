using AssistantContract.TgBot.Core.Src.View;
using AssistantContract.TgBot.Core.Src.View.Implementation;

namespace AssistantContract.TgBot.Di.View;

public class ViewBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IBotView, BaseBotView>();
        services.AddScoped<IBotView, StartBotView>();
        services.AddScoped<IBotView, AddContactView>();
        services.AddScoped<IBotView, ChangeContactView>();
    }
}
