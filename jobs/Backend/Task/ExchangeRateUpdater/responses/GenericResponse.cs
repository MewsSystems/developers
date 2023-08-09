using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExchangeRateUpdater.responses
{
    public class GenericResponse
    {
        [JsonProperty("is_ok")]
        public bool IsOKCode { get; set; }

        [JsonProperty("return_code")]
        public string ReturnCode { get; set; }

        [JsonProperty("return_message")]
        public string ReturnMessage { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("original_response")]
        public string OriginalResponse { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public HttpResponseMessage FullResponseMessage { get; set; }

        public dynamic ParseJsonBody() => ParseJsonBody<dynamic>();
        
        public T ParseJsonBody<T>()
        {
            if (IsOKCode)
            {
                try {
                    var converter = new IsoDateTimeConverter
                    {
                        DateTimeStyles = DateTimeStyles.AssumeUniversal
                    };
                    return JsonConvert.DeserializeObject<T>(OriginalResponse, converter);
                }
                catch
                {

                }
            }
            return default(T);
        }
    }
}