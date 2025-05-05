using AssistantContract.Application.UseCase.Contact.Commands.ChangeContact;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field.Controller;
using AssistantContract.TgBot.Core.Field.View;
using AssistantContract.TgBot.Core.Handler.BotStateTreeHandler;
using AssistantContract.TgBot.Core.Handler.BotStateTreeUserHandler;
using AssistantContract.TgBot.Core.Handler.BotViewHandler;
using AssistantContract.TgBot.Core.Model;
using AssistantContract.TgBot.Core.Model.Data;
using MediatR;

namespace AssistantContract.TgBot.Core.Src.Controller.Implementation;

public class ChangeContactController : IBotController
{
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly ISender _sender;
    public string Name() => ChangeContactField.ChangeContactState;

    public ChangeContactController(IBotStateTreeHandler botStateTreeHandler,
        IBotStateTreeUserHandler botStateTreeUserHandler,
        IBotViewHandler botViewHandler,
        ISender sender
    )
    {
        _botStateTreeHandler = botStateTreeHandler;
        _botStateTreeUserHandler = botStateTreeUserHandler;
        _botViewHandler = botViewHandler;
        _sender = sender;

        Initialize();
    }

    public async Task Exec(UpdateBDto update)
    {
        await _botStateTreeHandler.ExecuteRoute(update);
    }

    private void Initialize()
    {
        _botStateTreeHandler.AddAction(ChangeContactField.ChangeContactAction, ChangeContactAction);
        _botStateTreeHandler.AddAction(ChangeContactField.ChangeContactInputDescriptionAction, InputDescriptionAction);
        _botStateTreeHandler.AddKeyboard(ChangeContactField.ChangeContactInputDescriptionAction,
            ChangeContactField.ChangeContactSkipInputDescriptionKeyboard, SkipInputDescriptionAction);
        _botStateTreeHandler.AddAction(ChangeContactField.ChangeContactInputTimeSpanAction, InputTimeSpanAction);
        _botStateTreeHandler.AddCallback(ChangeContactField.ChangeContactInputTimeSpanAction,
            ChangeContactField.InputTimeSpanCallback, InputTimeSpanCallback);
        _botStateTreeHandler.AddCallback(ChangeContactField.ChangeContactInputTimeSpanAction,
            ChangeContactField.SkipInputTimeSpanCallback, SkipInputTimeSpanCallback);
        _botStateTreeHandler.AddKeyboard(ChangeContactField.ChangeContactSaveChangeContactAction,
            ChangeContactField.ChangeContactSavedContactKeyboard, SavedContactKeyboard);
        _botStateTreeHandler.AddKeyboard(ChangeContactField.ChangeContactSaveChangeContactAction,
            ChangeContactField.ChangeContactCanceledSaveContactKeyboard, CanceledSaveContactKeyboard);
    }


    private async Task ChangeContactAction(UpdateBDto arg)
    {
        var splitMessage = arg.GetMessage()!.Text!.Split(" ");
        var contactNumber = int.Parse(splitMessage[1]);
        ChangeContactDataDto data = new ChangeContactDataDto() { ContactNumber = contactNumber };
        await _botViewHandler.SendAsync(ChangeContactViewField.ChangeContactInputDescriptionAction, arg);
        await _botStateTreeUserHandler.SetDataAndActionAsync(arg,
            ChangeContactField.ChangeContactInputDescriptionAction, data);
    }

    private async Task InputDescriptionAction(UpdateBDto arg)
    {
        var text = arg.GetMessage().Text;
        if (!string.IsNullOrEmpty(text))
        {
            ChangeContactDataDto data = (await _botStateTreeUserHandler.GetDataAsync<ChangeContactDataDto>(arg))!;
            data.Description = text;
            await _botViewHandler.SendAsync(ChangeContactViewField.ChangeContactInputTimeSpan, arg);
            await _botStateTreeUserHandler.SetDataAndActionAsync(arg,
                ChangeContactField.ChangeContactInputTimeSpanAction, data);
        }
    }

    private async Task SkipInputDescriptionAction(UpdateBDto arg)
    {
        await _botViewHandler.SendAsync(ChangeContactViewField.ChangeContactInputTimeSpan, arg);
        await _botStateTreeUserHandler.SetActionAsync(arg,
            ChangeContactField.ChangeContactInputTimeSpanAction);
    }

    private async Task InputTimeSpanAction(UpdateBDto arg)
    {
        await _botViewHandler.SendAsync(ChangeContactViewField.ChangeContactInputTimeSpan, arg);
    }

    private async Task InputTimeSpanCallback(UpdateBDto arg)
    {
        var text = arg.CallbackData;
        if (!string.IsNullOrEmpty(text))
        {
            ChangeContactDataDto data = (await _botStateTreeUserHandler.GetDataAsync<ChangeContactDataDto>(arg))!;
            data.NotificationDayTimeSpan = int.Parse(text);
            await _botViewHandler.SendAsync(ChangeContactViewField.ChangeContactSaveChangeContact, arg);
            await _botStateTreeUserHandler.SetDataAndActionAsync(arg,
                ChangeContactField.ChangeContactSaveChangeContactAction, data);
        }
    }

    private async Task SkipInputTimeSpanCallback(UpdateBDto arg)
    {
        await _botViewHandler.SendAsync(ChangeContactViewField.ChangeContactSaveChangeContact, arg);
        await _botStateTreeUserHandler.SetActionAsync(arg,
            ChangeContactField.ChangeContactSaveChangeContactAction);
    }

    private async Task SavedContactKeyboard(UpdateBDto arg)
    {
        ChangeContactDataDto data = (await _botStateTreeUserHandler.GetDataAsync<ChangeContactDataDto>(arg))!;
        await _sender.Send(new ChangeContactCommand()
        {
            UserId = arg.GetUserId(),
            ContactNumber = data.ContactNumber,
            Description = data.Description!,
            NotificationDayTimeSpan = data.NotificationDayTimeSpan
        });
        await _botStateTreeUserHandler.ClearDataAsync(arg);
        await _botViewHandler.SendAsync(ChangeContactViewField.ChangeContactSavedContact, arg);
        await _botStateTreeUserHandler.SetStateAndActionAsync(arg, BaseField.BaseState, BaseField.BaseState);
    }

    private async Task CanceledSaveContactKeyboard(UpdateBDto arg)
    {
        await _botStateTreeUserHandler.ClearDataAsync(arg);
        await _botViewHandler.SendAsync(ChangeContactViewField.ChangeContactCanceledSaveContact, arg);
        await _botStateTreeUserHandler.SetStateAndActionAsync(arg, BaseField.BaseState, BaseField.BaseState);
    }
}
