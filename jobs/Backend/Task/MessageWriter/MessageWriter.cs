
namespace ExchangeRateUpdater.MessageWriter
{
    public class MessageWriter
    {
        public readonly IWriter Writer;
        public MessageWriter(MessageType type, string folderPath, string fileName = null)
        {

            switch (type)
            {
                case MessageType.Console:
                    {
                        Writer = new ConsoleWriter();
                        break;
                    };
                case MessageType.File:
                    {
                        Writer = new FileWriter(folderPath, fileName?? string.Empty);
                        break;
                    }
                default:
                    {
                        Writer = new ConsoleWriter();
                        break;
                    }

            }
        }

        public void WriteMessage(string message)
        {
            Writer.WriteMessage(message);
        }
    }
}
