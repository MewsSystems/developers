namespace Logger
{
    public interface ILogger
    {
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);
        void Info(string message);
        void Trace(string message);
        void Debug(string message);
    }
}