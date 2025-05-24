using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Field;
using AssistantContract.TgBot.Core.Model;
using Humanizer;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AssistantContract.TgBot.Core.Src.Command.Implementation;

public class TutorialCommand : IBotCommand
{
    private readonly ITelegramBotClient _telegramBotClient;
    
    public string GetCommand() => CommandField.Tutorial;
    public bool IsMoveNext() => false;

    public TutorialCommand(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task Exec(UpdateBDto update)
    {
        var tutorialMessage = """
        📚 *How to use this bot*:
        
        This bot helps you maintain meaningful connections with your contacts by sending you timely reminders and conversation starters.

        *Getting Started*:
        1. Use `/{0}` to add a new contact
        2. The bot will remind you when it's time to reach out
        3. Use `/{1}` before contacting someone for personalized conversation ideas
        4. View all contacts with `/{2}`
        5. Update contact info using `/{3}`
        6. Remove contacts you no longer need with `/{4}`

        *Pro Tip*: Type `/{5}` anytime to see all available commands!
        """.FormatWith(
            CommandField.AddContact,
            CommandField.GetRecommendation,
            CommandField.GetAllContacts,
            CommandField.ChangeContact,
            CommandField.DeleteMyContact,
            CommandField.Help
        );

        await _telegramBotClient.SendMessage(
            chatId: update.GetUserId(),
            text: tutorialMessage,
            parseMode: ParseMode.Markdown);
    }
}
