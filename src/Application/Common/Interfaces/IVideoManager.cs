namespace AssistantContract.Application.Common.Interfaces;

public interface IVideoManager
{
    Task<FileInfo> ExtractAudioAsync(string videoPath);
}
