namespace Logger
{
    // Basic file logger
    // TODO: make async and thread safe
    public class FileLogger : ILogger
    {
        public void Debug(string message)
        {
            File.AppendAllText($"{Constants.LogFileName}_{DateTime.Today.ToString("d-M-yyyy")}.{Constants.LogFileExtention}",
                $"{Constants.LogSeverities.DEBUG} -- {message} \r\n");
        }

        public void Error(string message)
        {
            File.AppendAllText($"{Constants.LogFileName}_{DateTime.Today.ToString("d-M-yyyy")}.{Constants.LogFileExtention}",
                $"{Constants.LogSeverities.ERROR} -- {message} \r\n");
        }

        public void Fatal(string message)
        {
            File.AppendAllText($"{Constants.LogFileName}_{DateTime.Today.ToString("d-M-yyyy")}.{Constants.LogFileExtention}",
                $"{Constants.LogSeverities.FATAL} -- {message} \r\n");
        }

        public void Info(string message)
        {
            File.AppendAllText($"{Constants.LogFileName}_{DateTime.Today.ToString("d-M-yyyy")}.{Constants.LogFileExtention}",
                $"{Constants.LogSeverities.INFO} -- {message} \r\n");
        }

        public void Trace(string message)
        {
            File.AppendAllText($"{Constants.LogFileName}_{DateTime.Today.ToString("d-M-yyyy")}.{Constants.LogFileExtention}",
                $"{Constants.LogSeverities.TRACE} -- {message} \r\n");
        }

        public void Warning(string message)
        {
            File.AppendAllText($"{Constants.LogFileName}_{DateTime.Today.ToString("d-M-yyyy")}.{Constants.LogFileExtention}",
                $"{Constants.LogSeverities.WARNING} -- {message} \r\n");
        }
    }
}
