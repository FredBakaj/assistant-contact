﻿namespace AssistantContract.TgBot.Core.Manager.UserFilter;

public interface IValidationManager
{
    bool IsAnyCyrillic(string text);
}
