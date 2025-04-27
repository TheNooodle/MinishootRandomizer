namespace MinishootRandomizer
{
    // ILogger interface is used to abstract the logging mechanism.
    // Log levels are based on the syslog standard : https://datatracker.ietf.org/doc/html/rfc5424.
    public interface ILogger
    {
        public void LogEmergency(string message);
        public void LogAlert(string message);
        public void LogCritical(string message);
        public void LogError(string message);
        public void LogWarning(string message);
        public void LogNotice(string message);
        public void LogInfo(string message);
        public void LogDebug(string message);
    }
}
