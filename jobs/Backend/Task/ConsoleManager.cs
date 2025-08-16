using System;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

public sealed class ConsoleManager : IConsoleManager
{
    public string ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(string line)
    {
        Console.WriteLine(line);
    }
}