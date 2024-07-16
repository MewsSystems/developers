using BenchmarkDotNet.Attributes;
using ExchangeRateUpdater.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Benchmark
{
    [MemoryDiagnoser]
    public class ExchangeRateMapperBenchmark
    {
        private readonly ExchangeRateMapper _mapper;
        private readonly string _data;

        public ExchangeRateMapperBenchmark()
        {
            _mapper = new ExchangeRateMapper();
            _data = LoadData();
        }

        private string LoadData()
        {
            // Generate or load your sample data here
            return @"12 Jul 2023 #134
                Country|Currency|Amount|Code|Rate
                Australia|dollar|1|AUD|15.531
                Brazil|real|1|BRL|4.717
                Bulgaria|lev|1|BGN|12.366
                Canada|dollar|1|CAD|17.509
                China|renminbi|1|CNY|3.241
                Denmark|krone|1|DKK|3.397
                EMU|euro|1|EUR|23.585
                Hong Kong|dollar|1|HKD|2.997
                Hungary|forint|100|HUF|6.431
                Iceland|krona|100|ISK|15.688
                IMF|SDR|1|XDR|30.858
                India|rupee|100|INR|27.928
                Indonesia|rupiah|1000|IDR|1.484
                Israel|new shekel|1|ILS|6.324
                Japan|yen|100|JPY|15.756
                Malaysia|ringgit|1|MYR|5.062
                Mexico|peso|1|MXN|1.102
                New Zealand|dollar|1|NZD|14.146
                Norway|krone|1|NOK|2.052
                Philippines|peso|100|PHP|4.156
                Poland|zloty|1|PLN|5.269
                Romania|leu|1|RON|4.837
                Russia|ruble|100|RUB|27.305
                Singapore|dollar|1|SGD|15.479
                South Africa|rand|1|ZAR|1.453
                South Korea|won|100|KRW|1.687
                Sweden|krona|1|SEK|2.212
                Switzerland|franc|1|CHF|24.195
                Thailand|baht|100|THB|6.340
                Turkey|lira|1|TRY|1.257
                Ukraine|hryvnia|1|UAH|0.646
                United Kingdom|pound|1|GBP|27.684
                USA|dollar|1|USD|21.069";
        }

        [Benchmark]
        public void Benchmark_Mapper_Map()
        {
            _mapper.Map(_data.AsSpan());
        }
    }
}
