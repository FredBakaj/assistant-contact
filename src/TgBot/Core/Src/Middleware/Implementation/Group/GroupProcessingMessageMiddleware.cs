using AssistantContract.Application.Common.Interfaces;
using AssistantContract.Application.Manager.Ai;
using AssistantContract.TgBot.Core.Extension;
using AssistantContract.TgBot.Core.Model;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AssistantContract.TgBot.Core.Src.Middleware.Implementation.Group;

public class GroupProcessingMessageMiddleware : ABotMiddleware
{
    private readonly ILogger<GroupProcessingMessageMiddleware> _logger;
    private readonly ITelegramBotClient _client;
    private readonly IVideoManager _videoManager;
    private readonly IAiManager _aiManager;


    private string _promptFirst = """
                                  Дай фидбек выступлению оратора. И напиши на сколько ты оценишь от 1 до 10
                                  
                                  {0}
                                  """;

    private string _promptSecond = """
                                   #Примеры
                                   Разговорная лексика:
                                   
                                   "Слушайте, ну мы же с вами прекрасно понимаем, что…"
                                   "Ну не бывает так, чтобы всё шло гладко, сами знаете."
                                   
                                   Риторические вопросы:
                                   
                                   "А вы разве не замечали, как это работает?"
                                   "Ну кто из нас не сталкивался с этим?"
                                   
                                   Простые фразы и повторы для усиления:
                                   
                                   "Это важно. Правда, очень важно."
                                   "Это не просто слова — это про нашу с вами жизнь."
                                   
                                   #Контекст
                                   Ты мастер оратарского искуства, который помогает начинающим улучшить свои навыки.
                                   #Задача
                                   Взять текст начинающего оратора и усилить его. Добавить убедительности. И сделать более интересными для слушателя.
                                   #Текст начинающего оратора
                                   {0}
                                   """;

    

    public GroupProcessingMessageMiddleware(ILogger<GroupProcessingMessageMiddleware> logger, ITelegramBotClient client,
        IVideoManager videoManager, IAiManager aiManager)
    {
        _logger = logger;
        _client = client;
        _videoManager = videoManager;
        _aiManager = aiManager;
    }

    public override async Task Next(UpdateBDto update)
    {
        if (update.Message?.Video != null || update.Message?.Voice != null)
        {
            try
            {
                string fileId = Guid.NewGuid().ToString();
                string filePath;
                FileInfo audioInfo;

                if (update.Message.Video != null)
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{fileId}.mp4");
                    var file = await _client.GetFile(update.Message.Video.FileId);
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await _client.DownloadFile(file.FilePath!, fileStream);
                    }

                    // Извлекаем аудио
                    audioInfo = await _videoManager.ExtractAudioAsync(filePath);
                    File.Delete(filePath); // Удаляем видео после извлечения аудио
                }
                else
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{fileId}.ogg");
                    var file = await _client.GetFile(update.Message.Voice!.FileId);
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await _client.DownloadFile(file.FilePath!, fileStream);
                    }

                    audioInfo = new FileInfo(filePath); // Используем аудиофайл напрямую
                }

                string audioFilePath = audioInfo.FullName;

                // Получаем транскрипцию и анализируем
                var transcription = await _aiManager.GetAudioTranscriptionAsync(audioFilePath);
                
                DateTime unixEpoch = new DateTime(1970, 1, 1);

                // Текущее время
                DateTime currentDate = DateTime.Now;

                // Разница между текущим временем и эпохой Unix
                TimeSpan difference = currentDate - unixEpoch;

                
                var analyzePrompt1 = await _aiManager.QueryToLlmModelAsync(string.Format(_promptFirst, transcription));
                
                var templateAnalyzePrompt1 = $"""
                                       <blockquote expandable>{analyzePrompt1}</blockquote>
                                       """;
                
                
                // Отправляем сообщение
                var userName = string.IsNullOrEmpty(update.Message.From!.Username)
                    ? "Воин"
                    : $"@{update.Message.From!.Username}";
                await _client.SendMessage(
                    chatId: update.GetChatId(),
                    text: $"Фидбек {userName} \n {templateAnalyzePrompt1}",
                    parseMode: ParseMode.Html
                );
                
                var analyzePrompt2 = await _aiManager.QueryToLlmModelAsync(string.Format(_promptSecond, transcription));

                var templateAnalyzePrompt2 = $"""
                                              <blockquote expandable>{analyzePrompt2}</blockquote>
                                              """;
                
                await _client.SendMessage(
                    chatId: update.GetChatId(),
                    text: $"улучшенный текст {userName} \n {templateAnalyzePrompt2}",
                    parseMode: ParseMode.Html
                );

                File.Delete(audioFilePath); // Удаляем аудиофайл
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Bad Request: file is too big"))
                {
                    await _client.SendMessage(
                        chatId: update.GetChatId(),
                        text: "Тяжело. Есть полегче? 20 мегабайт максимум 🤷🏼‍♂️"
                    );
                }
                else
                {
                    await _client.SendMessage(
                        chatId: update.GetChatId(),
                        text: $"Ошибка при обработке: {ex.Message}"
                    );
                }
            }
        }
    }
}
