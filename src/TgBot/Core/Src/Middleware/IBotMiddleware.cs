using AssistantContract.TgBot.Core.Model;

namespace AssistantContract.TgBot.Core.Src.Middleware
{
    /// <summary>
    /// Реалізація патерна ланцюжка обовязків
    /// </summary>
    public interface IBotMiddleware
    {
        /// <summary>
        /// Виклик наступного елемента ланцюжка обовязків
        /// </summary>
        public Task Next(UpdateBDto update);
        public void SetNext(IBotMiddleware next);
    }
}
