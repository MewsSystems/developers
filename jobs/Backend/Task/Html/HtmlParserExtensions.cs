using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Fluent;

namespace ExchangeRateUpdater.Html
{
    public static class HtmlParserExtensions
    {
        public static async Task<Fluent<StringElement>> ReadElementAsString(
            this Task<Fluent<Stream>> fluentStreamTask, ElementDescriptor elementDescriptor)
        {
            var fluentStream = await fluentStreamTask;
            var reader = new StreamReader(fluentStream.Value);
            fluentStream.State.AddDisposable(reader);
            
            var wasElementStartFound = false;
            var htmlText = new StringBuilder(); 

            var currentLine = await reader.ReadLineAsync();
            while (currentLine != null)
            {
                if (currentLine.Contains(elementDescriptor.GetStartElement()))
                {
                    wasElementStartFound = true;
                }

                if (wasElementStartFound)
                {
                    htmlText.AppendLine(currentLine);
                }

                if (currentLine.Contains(elementDescriptor.GetEndElement()))
                {
                    return fluentStream.Create(new StringElement(htmlText.ToString()));
                }

                currentLine = await reader.ReadLineAsync();
            }
            
            throw new Exception(wasElementStartFound 
                ? $"Table with tag definition '{elementDescriptor}' was not found"
                : $"Table with tag definition '{elementDescriptor}' has nto closing tag");
        }
    }
}