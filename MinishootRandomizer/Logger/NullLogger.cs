namespace MinishootRandomizer
{
    // NullLogger is a logger that does nothing. It is used to avoid null checks when a logger is not provided.
    public class NullLogger : ILogger
    {
        public void LogAlert(string message)
        {
        }

        public void LogCritical(string message)
        {
        }

        public void LogDebug(string message)
        {
        }

        public void LogEmergency(string message)
        {
        }

        public void LogError(string message)
        {
        }

        public void LogInfo(string message)
        {
        }

        public void LogNotice(string message)
        {
        }

        public void LogWarning(string message)
        {
        }
    }
}
