using ExchangeRateUpdater.Application.Banks;
using ExchangeRateUpdater.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Infrastructure.Factories
{
    public class BankFactory : IBankFactory
    {
        private readonly IEnumerable<IBankConnector> bankConnectors;

        public BankFactory(IEnumerable<IBankConnector> bankConnectors)
        {
            this.bankConnectors = bankConnectors;
        }

        public IBankConnector Create(BankIdentifier bankId)
        {
            var bankConnector = bankConnectors.FirstOrDefault(bank => bank.BankIdentifier == bankId);
            return bankConnector is null ? throw new NotSupportedException($"Bank '{bankId}' is not supported.") : bankConnector;
        }
    }
}
