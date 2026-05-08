using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests.UnitTests.Data
{
    public static class JsonTestDataLoader
    {
        public static string Load(string relativePath)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, "UnitTests", "Data", relativePath);
            return File.ReadAllText(fullPath);
        }
    }
}
