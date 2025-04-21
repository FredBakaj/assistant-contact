using AssistantContract.Application.UseCase.User.Commands.InitializeUser;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Model;
using MediatR;

namespace AssistantContract.TgBot.Core.Src.Middleware.Implementation.Main
{
    /// <summary>
    /// Первичная инициализация нового телеграмм аккаунта
    /// </summary>
    public class InitializationMiddleware : ABotMiddleware
    {
        private readonly ISender _sender;
        private readonly IConfiguration _configuration;

        public InitializationMiddleware(ISender sender, IConfiguration configuration)
        {
            _sender = sender;
            _configuration = configuration;
        }
        
        public override async Task Next(UpdateBDto update)
        {
            var interfaceLanguage = update.GetMessage()!.From!.LanguageCode;
            
            await _sender.Send(new InitializeUserCommand()
            {
                UserId = update.GetUserId(),
                State = StartField.StartState,
                Action = StartField.StartAction,
                InterfaceLanguage = interfaceLanguage!
            });
            await base.Next(update);
        }
    }
}
