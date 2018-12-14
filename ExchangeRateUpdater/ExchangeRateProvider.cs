using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    //Node which holds all the required info, but with amount being any number (later is processed in a separate method)
    public class ExRateOriginal
    {
        public int amount;
        public string code;
        public float rate;
    }

    public class ExchangeRateProvider
    {
        // For the enumerable type that GetExchangeRates will be returning I will use a list
        List<ExchangeRate> data = new List<ExchangeRate>();

        //this value that will be changing over the course of the program and pushed to the list
        static ExchangeRate exchangeRateNode;

        static WebClient client = new WebClient();

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            //defining regex's for searching for the text documents on the webpages
            Regex htmlMainDocFinder = new Regex("<a class=\"noprint\" href=\"(daily.txt.+?)\"");
            Regex htmlAdditionalDocFinder = new Regex("<a class=\"noprint\" href=\"(fx_rates.txt?.+?)\"");

            //providing the initial links for data extraction
            String MainRatesPageCode = client.DownloadString("https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.jsp");
            String AdditionalRatesPageCode = client.DownloadString("https://www.cnb.cz/en/financial_markets/foreign_exchange_market/other_currencies_fx_rates/fx_rates.jsp?month=12&year=2018");
            String MainRatesDocLink = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/" 
                + GetLink(MainRatesPageCode, htmlMainDocFinder);
            String AdditionalRatesDocLink = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/other_currencies_fx_rates/" 
                + GetLink(AdditionalRatesPageCode, htmlAdditionalDocFinder);

            //getting text from the corresponding documents
            var textDocMain = client.DownloadString(MainRatesDocLink);
            var textDocAdditional = client.DownloadString(AdditionalRatesDocLink);
            GetData(textDocMain, currencies);
            GetData(textDocAdditional, currencies);

            return data;
        }

        //almost main function here - calls all the other methods and finally modifies the empty data list
        private void GetData (String textDoc, IEnumerable<Currency> currencies)
        {
            StringReader stringReader = new StringReader(textDoc);

            string htmlLine = stringReader.ReadLine();
            htmlLine = stringReader.ReadLine();         //in the text doc that I parse two lines are useless

            while ((htmlLine = stringReader.ReadLine()) != null)
            {
                String[] dataSet = htmlLine.Split('|');
                CheckAndPush(MakeNode(dataSet));
                AddIfPresent(currencies);
            }    
        }

        // getting piece of html code from the webpage which leads to the text file we need
        private String GetLink(String htmlCode, Regex rgx) 
        {
            return rgx.Match(htmlCode).Groups[1].ToString();
        }

        //if any of the currencies coincides with the one we provide, add it to the report
        private void AddIfPresent(IEnumerable<Currency> currencies)
        {
            foreach (Currency currency in currencies)
            {
                if (currency.Code.Equals(exchangeRateNode.SourceCurrency.Code))
                {
                    data.Add(exchangeRateNode);
                    exchangeRateNode = new ExchangeRate(new Currency(""), new Currency(""), 0);
                    break;
                }
            }
        }

        //make an initial node - required, because the exchange amount may vary from 1 to 100
        private ExRateOriginal MakeNode (String[] dataSet) {

            ExRateOriginal temp = new ExRateOriginal();

            for (int j = 2; j < dataSet.Length; j++)
            {
                //pushing info to different fields of the ExRateOriginal class
                switch (j)
                {
                    case 2:
                        temp.amount = int.Parse(dataSet[j]);
                        break;
                    case 3:
                        temp.code = dataSet[j];
                        break;
                    case 4:
                        temp.rate = float.Parse(dataSet[j]);
                        break;
                }
            }

            return temp;
        }

        //check if amount > 1 and push to the list
        public void CheckAndPush(ExRateOriginal node)
        {
            if (node.amount > 1)
            {
                node.rate = node.rate / node.amount;
                node.amount = 1;
            }
            exchangeRateNode = new ExchangeRate(new Currency(node.code), new Currency("CZK"), (decimal)node.rate);
        }
    }
}
