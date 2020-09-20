using System;

namespace MvcMusicStore.Interfaces
{
    public interface ILogger_MusicStore
    {
        void LogDebug(string message);
        void LogInfo(string message);
        void LogError(Exception exception, string message);
    }
}
