namespace Logger
{
    internal static class Constants
    {
        public const string LogFileName = "log";
        public const string LogFileExtention = "txt";

        public enum LogSeverities
        {
            WARNING,
            ERROR,
            FATAL,
            INFO,
            TRACE,
            DEBUG
        }
    }
}
