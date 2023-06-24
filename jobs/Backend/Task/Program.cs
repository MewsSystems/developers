﻿using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToolsProvider.Helpers;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                IMapper mapper = MappingProvider.GetMapper();
                var provider = new ExchangeRateProvider(mapper);
                var rates = await provider.GetExchangeRates();

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates of commonly traded currencies:\n");

                foreach (var rate in rates)
                {
                    Console.Write(rate.Country + " " + rate.CurrencyName + " " + rate.Amount + " " + rate.Code + " " + rate.Rate);
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }    
    }
}
