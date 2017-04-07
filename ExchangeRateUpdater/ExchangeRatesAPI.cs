using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Oanda
{
    [DataContract]
    public class Currency
    {
        [DataMember (Name = "code")]
        public string Code { get; set; }
        [DataMember (Name = "description")]
        public string Description { get; set; }
    }

    [DataContract]
    public class EffectiveParams
    {
        [DataMember(Name = "decimal_places")]
        public int DecimalPlaces { get; set; }
        [DataMember(Name = "fields")]
        public List<string> Fields { get; set; }
        [DataMember(Name = "quote_currencies")]
        public List<string> QuoteCurrencies { get; set; }
        [DataMember(Name = "date")]
        private string DateStr {
            get
            {
                return _dateStr;
            }
            set
            {
                _dateStr = value;
                DateTime temp;
                if (DateTime.TryParse(_dateStr, out temp))
                {
                    Date = temp;
                }
            }
        }
        [DataMember(Name = "start")]
        private string StartDateStr { 
            get
            {
                return _startDateStr;
            }
            set 
            {
                _startDateStr = value;
                DateTime temp;
                if(DateTime.TryParse(_startDateStr, out temp))
                {
                    StartDate = temp;
                }                
            }
        }
        
        [DataMember(Name = "end")]
        private string EndDateStr {
            get
            {
                return _endDateStr;
            }
            set
            {
                _endDateStr = value;
                DateTime temp;
                if (DateTime.TryParse(_endDateStr, out temp))
                {
                    EndDate = temp;
                }
            }
        }

        private string _startDateStr;
        private string _dateStr;
        private string _endDateStr;
        

        public DateTime? Date { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        
    }

    [DataContract]
    public class Meta
    {   
        [DataMember(Name = "skipped_currencies")]
        public List<string> SkippedCurrencies { get; set; }
        [DataMember(Name = "effective_params")]
        public EffectiveParams EffectiveParams { get; set; }
        [DataMember(Name = "request_time")]
        private string RequestTimeStr
        {
            get
            {
                return _reqTimeStr;
            }
            set
            {
                _reqTimeStr = value;
                DateTime temp;
                if(DateTime.TryParse(_reqTimeStr, out temp))
                {
                    RequestTime = temp;
                }
            }
        }

        private string _reqTimeStr;
        public DateTime RequestTime { get; private set; }
    }

    [DataContract]
    public class Quote
    {
        private string _strDate;
        [DataMember (Name = "date")]
        private string strDate
        {
            get
            {
                return _strDate;
            }

            set
            {
                _strDate = value;
                DateTime temp;
                if (DateTime.TryParse(_strDate, out temp))
                {
                    Date = temp;
                }
            }
        }
        
        [DataMember (Name = "ask")]
        public double Ask { get; private set; }
        [DataMember(Name = "bid")]
        public double Bid { get; private set; }
        [DataMember(Name = "high_ask")]
        public double HighAsk { get; private set; }
        [DataMember(Name = "high_bid")]
        public double HighBid { get; private set; }
        [DataMember(Name = "low_ask")]
        public double LowAsk { get; private set; }
        [DataMember(Name = "low_bid")]
        public double LowBid { get; private set; }
        [DataMember(Name = "midpoint")]
        public double Midpoint { get; private set; }

        public DateTime? Date { get; private set; }
    }

    class Constants
    {
        public const string defaultBaseURL = "https://www.oanda.com/rates/api/v1/";
        public const string defaultProxyUrl = "";
        public const int defaultProxyPort = 8080;
    }

    [DataContract]
    public class ApiResponse
    {
        public bool IsSuccessful { get; internal set; }
        public string ErrorMessage { get; internal set; }
        public string RawJsonResponse { get; protected set; }

        public static T FromJson<T>(Stream json) where T : ApiResponse, new()
        {
            
            T serializedObj; 
            try
            {
                var sw = new StreamReader(json, System.Text.Encoding.ASCII);
                string jsonStr = sw.ReadToEnd();
                sw.Close();

                 using (var memStream = new MemoryStream())
                 {
                     var writer = new StreamWriter(memStream);
                     writer.Write(jsonStr);
                     writer.Flush();
                     memStream.Position = 0;
                     DataContractJsonSerializer ser = new DataContractJsonSerializer(
                                                            typeof(T), 
                                                            new DataContractJsonSerializerSettings { 
                                                            UseSimpleDictionaryFormat = true });

                     serializedObj = (T) ser.ReadObject(memStream);
                 }

                serializedObj.RawJsonResponse = jsonStr;
                serializedObj.IsSuccessful = true;
                serializedObj.ErrorMessage = "";
            }
            catch (Exception e)
            {
                serializedObj = new T();
                serializedObj.IsSuccessful = false;
                serializedObj.ErrorMessage = e.Message;
            }

            return serializedObj;
        }

        public ApiResponse(string errorMessage)
        {
            IsSuccessful = false;
            ErrorMessage = errorMessage;
        }

        public ApiResponse()
        {
            IsSuccessful = false;
            ErrorMessage = "Unspecified error";
        }

        public void SetErrorFromResponse(ErrorResponse errorResponse)
        {
            IsSuccessful = false;
            ErrorMessage = errorResponse.Message;
            RawJsonResponse = errorResponse.RawJsonResponse;
        }
    }

    [DataContract]
    public class RemainingQuotesResponse : ApiResponse
    {
        [DataMember(Name = "remaining_quotes")]
        public string RemainingQuotes { get; private set; }
    }

    [DataContract]
    public class GetCurrenciesResponse : ApiResponse
    {
        [DataMember(Name = "currencies")]
        public List<Currency> Currencies { get; private set; }
    }

    [DataContract]
    public class GetRatesResponse : ApiResponse
    {
        [DataMember(Name = "base_currency")]
        public string BaseCurrency { get; private set; }
        [DataMember(Name = "meta")]
        public Meta Meta { get; private set; }
        [DataMember(Name = "quotes")]
        public Dictionary<string, Quote> Quotes { get; private set; }
    }

    [DataContract]
    public class ErrorResponse : ApiResponse
    {
        [DataMember(Name = "code")]
        public string Code { get; private set; }
        [DataMember(Name = "message")]
        public string Message { get; private set; }
    }

    public class ExchangeRates
    {
        public string ApiKey; 
        public string BaseUrl; 
        public string ProxyUrl; 
        public int ProxyPort; 

        private T PerformRequest<T>(string requestName) where T : ApiResponse, new()
        {
            var url = String.Format("{0}{1}", BaseUrl, requestName);
            var request = (HttpWebRequest)WebRequest.Create(url);
            T response = new T();
            string credentialHeader = String.Format("Bearer {0}", ApiKey);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.UserAgent = "OANDAExchangeRates.C#/0.01";
            request.Headers.Add("Authorization", credentialHeader);

            if(ProxyUrl != "")
            {
                request.Proxy = new WebProxy(ProxyUrl, ProxyPort);
            }

            try
            {
                HttpWebResponse webresponse = (HttpWebResponse)request.GetResponse();
                response = ApiResponse.FromJson<T>(webresponse.GetResponseStream());                
            }
            catch (System.Net.WebException e)
            {
                var errorResponse = ApiResponse.FromJson<ErrorResponse>(e.Response.GetResponseStream());
                response.SetErrorFromResponse(errorResponse);
            }

            return response;
        }

        public ExchangeRates(string apiKey,
            string baseUrl = Constants.defaultBaseURL,
            string proxyUrl = Constants.defaultProxyUrl,
            int proxyPort = Constants.defaultProxyPort)
        {
            ApiKey = apiKey;
            BaseUrl = baseUrl;
            ProxyUrl = proxyUrl;
            ProxyPort = proxyPort;
        }

        public enum RatesFields
        {
            [Description("All")]
            All, 
            [Description("Averages")]
            Averages, 
            [Description("Midpoint")]
            Midpoint, 
            [Description("Highs")]
            Highs, 
            [Description("Lows")]
            Lows
        }

        private string GetRatesFieldsDescription(RatesFields en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute),
                        false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return "";
        }

        public GetRatesResponse GetRates(string baseCurrency, 
                List<string> quote = default(List<string>),
                List<RatesFields> fields = default(List<RatesFields>),
                string decimalPlaces = "",
                string date = "", 
                string start = "",
                string end = "")
        {
            GetRatesResponse response = null;

            try
            {
                string queryStr = RatesParamaterQueryString(quote, fields, decimalPlaces, date, start, end);
                string requestStr = string.Format("rates/{0}.json{1}", baseCurrency, queryStr); 
                response = PerformRequest<GetRatesResponse>(requestStr);
            }
            catch (Exception exception)
            {
                response = new GetRatesResponse();
                response.IsSuccessful = false;
                response.ErrorMessage = exception.Message;
            }

            return response;
        }

        public GetRatesResponse GetRates(string baseCurrency,
                string quote = "",
                RatesFields fields = default(RatesFields),
                string decimalPlaces = "",
                string date = "",
                string start = "",
                string end = "")
        {
            return GetRates(baseCurrency,
                new List<string> { quote },
                new List<RatesFields> { fields },
                decimalPlaces,
                date,
                start,
                end);
        }

        public GetRatesResponse GetRates(string baseCurrency,
                List<string> quote = default(List<string>),
                RatesFields fields = default(RatesFields),
                string decimalPlaces = "",
                string date = "",
                string start = "",
                string end = "")
        {
            return GetRates(baseCurrency,
                quote ,
                new List<RatesFields> { fields },
                decimalPlaces,
                date,
                start,
                end);
        }

        public GetRatesResponse GetRates(string baseCurrency,
                string quote = "",
                List<RatesFields> fields = default(List<RatesFields>),
                string decimalPlaces = "",
                string date = "",
                string start = "",
                string end = "")
        {
            return GetRates(baseCurrency,
                new List<string> { quote },
                fields,
                decimalPlaces,
                date,
                start,
                end);
        }

        private string RatesParamaterQueryString(
                List<string> quote = default(List<string>),
                List<RatesFields> fields = default(List<RatesFields>),
                string decimalPlaces = "",
                string date = "", 
                string start = "",
                string end = "")
            {
                string response= "";

                var prms = new List<KeyValuePair<string, string>>();//new Dictionary<string, string>();
                var nonDefaultParams = new List<KeyValuePair<string, string>>();

                foreach(var q in quote)
                {
                    prms.Add(new KeyValuePair<string, string>("quote", q));
                }

                foreach( var f in fields)
                {
                    prms.Add(new KeyValuePair<string, string>("fields", GetRatesFieldsDescription(f)));
                }

                prms.Add(new KeyValuePair<string, string>("decimal_places", decimalPlaces));
                prms.Add(new KeyValuePair<string, string>("date", date));
                prms.Add(new KeyValuePair<string, string>("start", start));
                prms.Add(new KeyValuePair<string, string>("end", end));

                foreach (var item in prms)
                {
                    if( item.Value != "")
                    {
                        nonDefaultParams.Add(new KeyValuePair<string, string>(item.Key, item.Value)); 
                    }
                }

                bool first = true;

                foreach (var par in nonDefaultParams)
                {
                    string separator = "&";
                    
                    if(first)
                    {
                        separator = "?";
                        first = false;
                    }

                    response = string.Format("{0}{1}{2}={3}", response, separator, par.Key, par.Value);
                }
                
                return response;
            }

        public GetCurrenciesResponse GetCurrencies()
        {
            GetCurrenciesResponse response = null;

            try
            {
                response = PerformRequest<GetCurrenciesResponse>("currencies.json");
            }
            catch (Exception exception)
            {
                response = new GetCurrenciesResponse();
                response.IsSuccessful = false;
                response.ErrorMessage = exception.Message;
            }

            return response;
        }

        public RemainingQuotesResponse GetRemainingQuotes()
        {
            RemainingQuotesResponse response = null;

            try
            {
                response = PerformRequest<RemainingQuotesResponse>("remaining_quotes.json");
            }
            catch (Exception exception)
            {
                response = new RemainingQuotesResponse();
                response.IsSuccessful = false;
                response.ErrorMessage = exception.Message;
            }

            return response;
        }
    }

}
