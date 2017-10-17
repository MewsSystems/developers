using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    class FileParser
    {
        public List<ExchangeRate> GetDataFromServer(DataSourceEnum dataSource)
        {
            List<ExchangeRate> eRates = new List<ExchangeRate>();
            switch (dataSource)
            {
                case DataSourceEnum.CzechNationalBank:
                    return GetRatesFromCzechNationalBank();
                case DataSourceEnum.EuropeCentralBank:
                    return GetRatesFromEuropeCentralBank();
                case DataSourceEnum.CustomsAdministrationXML:
                    return GetRatesFromCustomsAdministration(true);
                case DataSourceEnum.CustomsAdministrationTXT:
                    return GetRatesFromCustomsAdministration();
                default:
                    throw new ArgumentException("Not implemented datasource parser");
            }
        }

        /// <summary>
        /// Returning data from xml or txt depend on bool
        /// </summary>
        /// <param name="fileType">If true return data from xml. Default return data from txt.</param>
        private List<ExchangeRate> GetRatesFromCustomsAdministration(bool fileType=false)
        {
            throw new NotImplementedException();
        }

        private List<ExchangeRate> GetRatesFromEuropeCentralBank()
        {
            throw new NotImplementedException();
        }

        private List<ExchangeRate> GetRatesFromCzechNationalBank()
        {
            throw new NotImplementedException();
        }
    }
}
