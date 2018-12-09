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
    public class ERTMP      //ExchangeRateTeMPorary - this class is just a tool to get initial info and process it
    {
        public int am;      //amount
        public string cd;   //code
        public float rt;      //rate
    }

    public class ExchangeRateProvider
    {
        // For the enumerable type that GetExchangeRates will be returning I will use a list
        List<ExchangeRate> data = new List<ExchangeRate>();

        //er - exchangeRate - this value that will be changing over the course of the program and pushed to the list
        static ExchangeRate er;

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // getting html code from the webpage
            var client = new WebClient();
            var text = client.DownloadString(
                "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.jsp");

            // processing the html code string, using regular expressions
            StringReader sr = new StringReader(text);
            string s = null;
            Regex rgx = new Regex(@"kurzy_tisk");

            // I studied the initial html code and after the lines with "kurzy tisk" there are a few lines with
            // the codes and rates that we need, so I just made a simple loop which breaks as soon as I find those
            // works as follows: finds "kurzy tisk", then for 20 lines (enough to read all rates) processes another
            // regular expression to extract the required info and parses to another method
            while (true)
            {
                s = sr.ReadLine();
                if (rgx.IsMatch(s))
                {
                    for (int i = 0; i < 20; i++)
                    {
                        String k = sr.ReadLine();
                        MatchCollection mc = Regex.Matches(k, @"(<td>|<td align=.right.>)(.+?)</td>");
                        int c = 0;      // iterator for matches of the regex (because the expression extracts some extra info which we need to dispose of)
                        int y = 0;      // iterator for the insides of ertmp class
                        ERTMP tmp = new ERTMP();

                        foreach (Match m in mc)
                        {
                            if (c % 5 < 2)      // we don't need names of the countries and names of their currencies
                            {
                                y = 0;
                                c++;
                                continue;
                            }
                            else
                            {
                                String x = m.Groups[2].Value;       //once we reach what we need, we push it to the corresponding values of a node

                                //pushing info to different fields of the ertmp class
                                switch (y)
                                {
                                    case 0:
                                        tmp.am = int.Parse(x);
                                        break;
                                    case 1:
                                        tmp.cd = x;
                                        break;
                                    case 2:
                                        tmp.rt = float.Parse(x);
                                        break;
                                }

                                // if our temp class is fully filled with info, we process it, push to the list and renew
                                if (CheckAndPush(tmp))
                                {
                                    tmp = new ERTMP();
                                    foreach (Currency cu in currencies)
                                    {
                                        if (cu.Code.Equals(er.SourceCurrency.Code))
                                        {
                                            data.Add(er);
                                            er = new ExchangeRate(new Currency(""), new Currency(""), 0);
                                            break;
                                        }
                                    }
                                }
                                c++;
                                y++;
                            }
                        }
                    }
                    break;
                }
            }

            return data;
        }

        //check if the ertmp node is fully filled in; if so - update er object
        public bool CheckAndPush(ERTMP node)
        {
            if (node.am == 0 || node.cd == null || node.rt == 0.0F) return false;
            if (node.am > 1)
            {
                node.rt = node.rt / node.am;
                node.am = 1;
            }
            er = new ExchangeRate(new Currency(node.cd), new Currency("CZK"), (decimal)node.rt);
            return true;
        }
    }
}
