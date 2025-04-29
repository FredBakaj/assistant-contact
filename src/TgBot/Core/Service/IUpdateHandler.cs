﻿using Telegram.Bot;
using Telegram.Bot.Types;

namespace AssistantContract.TgBot.Core.Service;

public interface IUpdateHandler
{
    Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken);
}
