using ExchangeRateUpdater.MessageWriter;
using System;
using System.Configuration;

namespace ExchangeRateUpdater.Log
{
    public class Logger : ILogger
    {
        private readonly string logFolder = ConfigurationManager.AppSettings["LogFolderPath"];
        private IWriter _writer;

        public Logger(MessageType type)
        {
            _writer = new MessageWriter.MessageWriter(type, logFolder).Writer;
        }
        public void LogInfo(string message)
        {
            _writer.WriteMessage($"{DateTime.Now.ToString("dd.MM.yyyyy:hh:mm:ss.f")} - Info: {message}");
        }

        public void LogError(string message)
        {
            _writer.WriteMessage($"{DateTime.Now.ToString("dd.MM.yyyyy:hh:mm:ss.f")} - Error: {message}");
        }
    }
}
