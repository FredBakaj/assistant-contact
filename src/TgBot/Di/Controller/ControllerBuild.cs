using AssistantContract.TgBot.Core.Src.Controller;
using AssistantContract.TgBot.Core.Src.Controller.Implementation;

namespace AssistantContract.TgBot.Di.Controller
{
    public class ControllerBuild
    {
        public static void BuildService(IServiceCollection services)
        {
            services.AddScoped<IBotController, BaseController>();
            services.AddScoped<IBotController, StartController>();
            services.AddScoped<IBotController, AddContactController>();

        }
    }
}
