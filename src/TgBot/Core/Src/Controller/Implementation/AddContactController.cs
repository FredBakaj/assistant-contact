using System.Security.Cryptography;
using AssistantContract.Application.UseCase.Contact.Commands.AddContact;
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

public class AddContactController : IBotController
{
    private readonly IBotStateTreeHandler _botStateTreeHandler;
    private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
    private readonly IBotViewHandler _botViewHandler;
    private readonly ISender _sender;
    public string Name() => AddContactField.AddContactState;

    public AddContactController(IBotStateTreeHandler botStateTreeHandler,
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
        _botStateTreeHandler.AddAction(AddContactField.AddContactAction, AddContactAction);
        _botStateTreeHandler.AddAction(AddContactField.InputNameAction, InputNameAction);
        _botStateTreeHandler.AddAction(AddContactField.InputPhoneAction, InputPhoneAction);
        _botStateTreeHandler.AddKeyboard(AddContactField.InputPhoneAction, AddContactField.SkipInputPhoneKeyboard,
            SkipInputPhoneKeyboard);

        _botStateTreeHandler.AddAction(AddContactField.InputDescriptionAction, InputDescriptionAction);
        _botStateTreeHandler.AddKeyboard(AddContactField.SaveContactAction, AddContactField.SavedContactKeyboard,
            SavedContactKeyboard);
        _botStateTreeHandler.AddKeyboard(AddContactField.SaveContactAction, AddContactField.CanceledSaveContactKeyboard,
            CanceledSaveContactKeyboard);
    }

    private async Task AddContactAction(UpdateBDto arg)
    {
        await _botViewHandler.SendAsync(AddContactViewField.InputName, arg);
        await _botStateTreeUserHandler.SetActionAsync(arg, AddContactField.InputNameAction);
    }

    private async Task InputNameAction(UpdateBDto arg)
    {
        var text = arg.GetMessage().Text;
        if (!string.IsNullOrEmpty(text))
        {
            AddContactDataDto data = new AddContactDataDto() { Name = text };
            await _botViewHandler.SendAsync(AddContactViewField.InputPhone, arg);
            await _botStateTreeUserHandler.SetDataAndActionAsync(arg, AddContactField.InputPhoneAction, data);
        }
    }

    private async Task InputPhoneAction(UpdateBDto arg)
    {
        var text = arg.GetMessage().Text;
        if (!string.IsNullOrEmpty(text))
        {
            AddContactDataDto data = (await _botStateTreeUserHandler.GetDataAsync<AddContactDataDto>(arg))!;
            data.Phone = text;
            await _botViewHandler.SendAsync(AddContactViewField.InputDescription, arg);
            await _botStateTreeUserHandler.SetDataAndActionAsync(arg, AddContactField.InputDescriptionAction, data);
        }
    }

    private async Task SkipInputPhoneKeyboard(UpdateBDto arg)
    {
        await _botViewHandler.SendAsync(AddContactViewField.InputDescription, arg);
        await _botStateTreeUserHandler.SetActionAsync(arg, AddContactField.InputDescriptionAction);
    }


    private async Task InputDescriptionAction(UpdateBDto arg)
    {
        var text = arg.GetMessage().Text;
        if (!string.IsNullOrEmpty(text))
        {
            AddContactDataDto data = (await _botStateTreeUserHandler.GetDataAsync<AddContactDataDto>(arg))!;
            data.Description = text;
            await _botViewHandler.SendAsync(AddContactViewField.SaveContact, arg);
            await _botStateTreeUserHandler.SetDataAndActionAsync(arg, AddContactField.SaveContactAction, data);
        }
    }

    private async Task SavedContactKeyboard(UpdateBDto arg)
    {
        AddContactDataDto data = (await _botStateTreeUserHandler.GetDataAsync<AddContactDataDto>(arg))!;
        await _sender.Send(new AddContactCommand()
        {
            UserId = arg.GetUserId(), Name = data.Name!, Phone = data.Phone, Description = data.Description!
        });
        await _botViewHandler.SendAsync(AddContactViewField.SavedContact, arg);
        await _botStateTreeUserHandler.SetStateAndActionAsync(arg, BaseField.BaseState, BaseField.BaseState);
    }

    private async Task CanceledSaveContactKeyboard(UpdateBDto arg)
    {
        await _botStateTreeUserHandler.ClearDataAsync(arg);
        await _botViewHandler.SendAsync(AddContactViewField.CanceledSaveContact, arg);
        await _botStateTreeUserHandler.SetStateAndActionAsync(arg, BaseField.BaseState, BaseField.BaseState);
    }
}
