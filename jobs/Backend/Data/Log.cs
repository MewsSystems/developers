using Newtonsoft.Json;
using Serilog;

namespace Data;

public interface ILog<T> where T : class
{
    void Error(string message);

    void Error(string message, object objectToSerialize);

    void Error(string message, Exception exception);

    void Error(string message, object objectToSerialize, Exception exception);

    public void Info(string message);

    public void Info(string message, object objectToSerialize);
}

public class Log<T> : ILog<T> where T : class
{
    private const string AppName = "ExchangeRate: ";
    private ILogger _logger;

    public Log(ILogger logger)
    {
        _logger = logger.ForContext<T>();
    }

    public void Error(string message)
    {
        _logger.Error($"{AppName}{message}");
    }

    public void Error(string message, Exception exception)
    {
        Error($"{message} - {exception.Message} - {exception.StackTrace}");
    }

    public void Error(string message, object objectToSerialize)
    {
        Error($"{message} - {JsonConvert.SerializeObject(objectToSerialize)}");
    }

    public void Error(string message, object objectToSerialize, Exception exception)
    {
        Error($"{message} - {JsonConvert.SerializeObject(objectToSerialize)}");
        Error($"Exception: {exception.Message} - {exception.StackTrace}");
    }

    public void Info(string message)
    {
        _logger.Information($"{AppName}{message}");
    }

    public void Info(string message, object objectToSerialize)
    {
        Info($"{message} - {JsonConvert.SerializeObject(objectToSerialize)}");
    }
}
