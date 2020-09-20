using NLog;
using System;
using MvcMusicStore.Interfaces;

namespace MvcMusicStore.Models
{
    public class Logger: ILogger_MusicStore
    {
        private readonly NLog.ILogger _logger;
        public Logger()
        {
            _logger = LogManager.GetLogger("Logger"); 
        }

        

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }
        public void LogError(Exception exception, string message)
        {
            _logger.Error(exception, message);
        }


      
    }
}
