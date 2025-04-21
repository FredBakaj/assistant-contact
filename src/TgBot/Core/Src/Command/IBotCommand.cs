using AssistantContract.TgBot.Core.Model;

namespace AssistantContract.TgBot.Core.Src.Command;

public interface IBotCommand
{
    /// <summary>
    /// Для получения имя команды, по которому будет вызван метод Exec
    /// </summary>
    /// <returns>Имя команды</returns>
    public string GetCommand();
    /// <summary>
    /// Стоит ли продолжать вызов Next Middleware
    /// </summary>
    /// <returns></returns>
    public bool IsMoveNext();
    /// <summary>
    /// Реализация логики команды (как правило, изменяет состояние пользователя)
    /// </summary>
    /// <returns></returns>
    public Task Exec(UpdateBDto update);

}
