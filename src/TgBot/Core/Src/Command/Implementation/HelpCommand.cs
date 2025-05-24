using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Model;
using Humanizer;
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
        var helpMessage = """
        🤖 *Available Commands*:

        *Getting Started*
        • /{0} - Show this help message
        • /{1} - Learn how to use this bot
        • /{2} - Start the bot

        *Contact Management*
        • /{3} - Add a new contact
        • /{4} - List all your contacts
        • /{5} - Change contact information
        • /{6} [number] - Delete a contact
        • /{7} [number] - Get conversation ideas for a contact

        *Examples*
        ```
        /{3} - Add a new contact
        /{6} 1 - Delete contact #1
        /{7} 2 - Get ideas for contact #2
        ```
        """.FormatWith(
            CommandField.Help,
            CommandField.Tutorial,
            CommandField.Start,
            CommandField.AddContact,
            CommandField.GetAllContacts,
            CommandField.ChangeContact,
            CommandField.DeleteMyContact,
            CommandField.GetRecommendation
        );

        await _telegramBotClient.SendMessage(
            chatId: update.GetUserId(),
            text: helpMessage,
            parseMode: ParseMode.Markdown);
    }
}
