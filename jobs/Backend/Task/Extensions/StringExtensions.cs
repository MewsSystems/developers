using System.Text;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex MultiSpaceRegex = new(@"\s+", RegexOptions.Compiled);

        public static string ToSentenceCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string[] words = MultiSpaceRegex.Split(text);
            StringBuilder sentenceCase = new();

            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    sentenceCase.Append(char.ToUpper(word[0]));
                    sentenceCase.Append(word[1..].ToLower());
                    sentenceCase.Append(' ');
                }
            }

            return sentenceCase.ToString().TrimEnd();
        }
    }
}