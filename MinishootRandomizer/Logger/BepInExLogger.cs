using BepInEx.Logging;

namespace MinishootRandomizer
{
    public class BepInExLogger : ILogger
    {
        private ManualLogSource _innerLogger;

        public BepInExLogger(ManualLogSource innerLogger)
        {
            _innerLogger = innerLogger;
        }

        public void LogAlert(string message)
        {
            _innerLogger.Log(LogLevel.Fatal, message);
        }

        public void LogCritical(string message)
        {
            _innerLogger.Log(LogLevel.Fatal, message);
        }

        public void LogDebug(string message)
        {
            _innerLogger.Log(LogLevel.Debug, message);
        }

        public void LogEmergency(string message)
        {
            _innerLogger.Log(LogLevel.Fatal, message);
        }

        public void LogError(string message)
        {
            _innerLogger.Log(LogLevel.Error, message);
        }

        public void LogInfo(string message)
        {
            _innerLogger.Log(LogLevel.Info, message);
        }

        public void LogNotice(string message)
        {
            _innerLogger.Log(LogLevel.Message, message);
        }

        public void LogWarning(string message)
        {
            _innerLogger.Log(LogLevel.Warning, message);
        }
    }
}
