using AssistantContract.TgBot.Core.Model;

namespace AssistantContract.TgBot.Core.Manager.UserFilter
{
    public interface ISpamQueryManager
    {
        Task<bool> IsNoReachLimit(long userId);
        Task AddRecord(SpamQueryBDto spamQuery);
    }
}
