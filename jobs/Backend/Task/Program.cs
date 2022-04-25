using System;

namespace ExchangeRateUpdater
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                // Init. services
                new Startup(args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }
        }
    }
}
