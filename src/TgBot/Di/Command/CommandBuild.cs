using AssistantContract.TgBot.Core.Src.Command;
using AssistantContract.TgBot.Core.Src.Command.Implementation;

namespace AssistantContract.TgBot.Di.Command;

public class CommandBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IBotCommand, StartCommand>();
        services.AddScoped<IBotCommand, ReloadCommand>();
        services.AddScoped<IBotCommand, AddContactCommand>();
        services.AddScoped<IBotCommand, GetRecommendationCommand>();
        services.AddScoped<IBotCommand, GetAllContactsCommand>();
    }
}
