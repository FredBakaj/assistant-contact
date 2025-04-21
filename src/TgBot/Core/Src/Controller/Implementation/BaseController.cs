using AutoMapper;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Handler.BotStateTreeHandler;
using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler;
using AssistantContract.TgBot.Core.Handler.BotViewHandler;
using AssistantContract.TgBot.Core.Model;
using MediatR;

namespace AssistantContract.TgBot.Core.Src.Controller.Implementation
{
    public class BaseController : IBotController
    {
        private readonly IBotStateTreeHandler _botStateTreeHandler;



        public string Name() => BaseField.BaseState;

        public BaseController(IBotStateTreeHandler botStateTreeHandler)
        {
            _botStateTreeHandler = botStateTreeHandler;



            Initialize();
        }

        public async Task Exec(UpdateBDto update)
        {
            await _botStateTreeHandler.ExecuteRoute(update);
        }

        private void Initialize()
        {
            _botStateTreeHandler.AddAction(BaseField.BaseAction, BaseAction);
        }


        //Добавление предложений в базу 
        private async Task BaseAction(UpdateBDto update)
        {
            await Task.CompletedTask;
        }
    }
}
