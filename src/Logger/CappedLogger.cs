namespace MinishootRandomizer;

public class CappedLogger : ILogger
{
    private enum LogLevel
    {
        Debug,
        Info,
        Notice,
        Warning,
        Error,
        Critical,
        Alert,
        Emergency
    }

    private readonly ILogger _innerLogger;

    private LogLevel _minLevel = LogLevel.Info;

    public CappedLogger(ILogger innerLogger)
    {
        _innerLogger = innerLogger;
    }

    public void LogAlert(string message)
    {
        if (LogLevel.Alert >= _minLevel)
        {
            _innerLogger.LogAlert(message);
        }
    }

    public void LogCritical(string message)
    {
        if (LogLevel.Critical >= _minLevel)
        {
            _innerLogger.LogCritical(message);
        }
    }

    public void LogDebug(string message)
    {
        if (LogLevel.Debug >= _minLevel)
        {
            _innerLogger.LogDebug(message);
        }
    }

    public void LogEmergency(string message)
    {
        if (LogLevel.Emergency >= _minLevel)
        {
            _innerLogger.LogEmergency(message);
        }
    }

    public void LogError(string message)
    {
        if (LogLevel.Error >= _minLevel)
        {
            _innerLogger.LogError(message);
        }
    }

    public void LogInfo(string message)
    {
        if (LogLevel.Info >= _minLevel)
        {
            _innerLogger.LogInfo(message);
        }
    }

    public void LogNotice(string message)
    {
        if (LogLevel.Notice >= _minLevel)
        {
            _innerLogger.LogNotice(message);
        }
    }

    public void LogWarning(string message)
    {
        if (LogLevel.Warning >= _minLevel)
        {
            _innerLogger.LogWarning(message);
        }
    }
}
