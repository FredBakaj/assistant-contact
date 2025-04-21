using System.Reflection;
using AssistantContract.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Xabe.FFmpeg;

namespace AssistantContract.Infrastructure.Media;

public class VideoManager : IVideoManager
{
    public VideoManager(IConfiguration configuration)
    {
        string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                              AppDomain.CurrentDomain.BaseDirectory;
        string ffmpegPath = Path.Combine(appDirectory, "ffmpeg", "bine");
        FFmpeg.SetExecutablesPath(ffmpegPath);
    }

    public async Task<FileInfo> ExtractAudioAsync(string videoPath)
    {
        if (!File.Exists(videoPath))
        {
            throw new FileNotFoundException("Video file not found", videoPath);
        }

        // Создаем уникальное имя для аудиофайла
        string audioFileId = Guid.NewGuid().ToString();
        string audioFilePath = Path.Combine(Path.GetDirectoryName(videoPath)!, $"{audioFileId}.mp3");

        // Извлекаем аудио
        var conversion = await FFmpeg.Conversions.New()
            .AddParameter($"-i \"{videoPath}\" -vn -acodec mp3 \"{audioFilePath}\"")
            .Start();

        if (!File.Exists(audioFilePath))
        {
            throw new InvalidOperationException("Failed to extract audio");
        }

        return new FileInfo(audioFilePath);
    }
}
