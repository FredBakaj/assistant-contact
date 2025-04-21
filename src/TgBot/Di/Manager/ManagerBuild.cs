using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Infrastructure.Config;
using AssistantContract.TgBot.Core.Manager.UserFilter;
using AssistantContract.TgBot.Core.Manager.UserFilter.Implementation;

namespace AssistantContract.TgBot.Di.Manager;

public class ManagerBuild
{
    public static void BuildService(IServiceCollection services)
    {
        services.AddScoped<IConfig, Config>();
        services.AddScoped<ISpamQueryManager, SpamQueryManager>();
        services.AddScoped<IValidationManager, ValidationManager>();
    }
}
