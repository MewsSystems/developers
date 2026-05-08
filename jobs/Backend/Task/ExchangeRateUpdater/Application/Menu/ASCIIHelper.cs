using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Menu
{
    public static class ASCIIHelper
    {
        public static string LoadFromAssets(string relativePath)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"Unable to load ascii asset {fullPath}");
                return null;
            }

            return File.ReadAllText(fullPath);
        }
    }
}
