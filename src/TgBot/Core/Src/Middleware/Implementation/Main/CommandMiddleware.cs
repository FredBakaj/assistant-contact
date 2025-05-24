using AssistantContract.TgBot.Core.Factory;
using AssistantContract.TgBot.Core.Model;
using AssistantContract.TgBot.Core.Src.Command;
using Microsoft.Extensions.DependencyInjection;
using System;
using AssistantContract.TgBot.Core.Extension;
using Telegram.Bot;

namespace AssistantContract.TgBot.Core.Src.Middleware.Implementation.Main;

/// <summary>
/// Обработака команд пользователя
/// </summary>
public class CommandMiddleware : ABotMiddleware
{
    private readonly IFactory<IBotCommand> _commandFactory;
    private readonly ILogger<CommandMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CommandMiddleware(
        IFactory<IBotCommand> commandFactory,
        ILogger<CommandMiddleware> logger,
        IServiceProvider serviceProvider)
    {
        _commandFactory = commandFactory;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override async Task Next(UpdateBDto update)
    {
        try
        {
            bool isMoveNext = true;
            if (update.Message != null
                && update.Message.Text != null
                && update.Message.Text.StartsWith("/"))
            {
                var commandText = update.Message.Text.Substring(1);
                var command = await _commandFactory.CreateAsync(commandText);
                isMoveNext = command.IsMoveNext();
                await command.Exec(update);
            }

            if (isMoveNext)
            {
                await base.Next(update);
            }
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("sequence contains no matching element",
                                                       StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation(ex, "Unknown command received");
            if (update.Message != null)
            {
                try
                {
                    var botClient = _serviceProvider.GetRequiredService<ITelegramBotClient>();
                    await botClient.SendMessage(
                        chatId: update.GetUserId(),
                        text: "❌ This bot does not have this command. Type /help to see available commands.");
                }
                catch (Exception sendEx)
                {
                    _logger.LogError(sendEx, "Failed to send unknown command message");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing command");
        }
    }
}
