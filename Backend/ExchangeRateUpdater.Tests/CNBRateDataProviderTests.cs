using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class CNBRateDataProviderTests
    {
        [Fact]
        public void CheckProvidedCNBCurrenciesCount()
        {
            var cnbData = new CNBRateDataProvider();
            Assert.Equal(156, cnbData.Data.Keys.Count);
        }
    }
}
