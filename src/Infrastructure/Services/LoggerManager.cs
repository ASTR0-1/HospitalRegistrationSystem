using HospitalRegistrationSystem.Application.Interfaces;
using NLog;

namespace HospitalRegistrationSystem.Infrastructure.Services;

public class LoggerManager : ILoggerManager
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public void LogDebug(string message)
    {
        Logger.Debug(message);
    }

    public void LogError(string message)
    {
        Logger.Error(message);
    }

    public void LogInformation(string message)
    {
        Logger.Info(message);
    }

    public void LogWarning(string message)
    {
        Logger.Warn(message);
    }
}