using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AssistantContract.TgBot.Core.Src.Command.Implementation;

public class HelpCommand : IBotCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    
    public string GetCommand() => CommandField.Help;
    public bool IsMoveNext() => false;

    public HelpCommand(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task Exec(UpdateBDto update)
    {
        var helpMessage = 
            "🤖 *Available Commands*:\n\n" +
            $"• /{CommandField.Start} - Start the bot\n" +
            $"• /{CommandField.Help} - Show this help message\n" +
            $"• /{CommandField.AddContact} - Add a new contact\n" +
            $"• /{CommandField.GetAllContacts} - List all your contacts\n" +
            $"• /{CommandField.ChangeContact} - Change contact information\n" +
            $"• /{CommandField.DeleteMyContact} [number] - Delete a contact\n" +
            $"• /{CommandField.GetRecommendation} [number] - Get recommendations for a contact\n\n" +
            "Example usage:\n" +
            $"`/{CommandField.AddContact}` - Add a new contact\n" +
            $"`/{CommandField.DeleteMyContact} 1` - Delete contact #1";

        await _telegramBotClient.SendMessage(
            chatId: update.GetUserId(),
            text: helpMessage,
            parseMode: ParseMode.Markdown);
    }
}
