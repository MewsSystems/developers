using Newtonsoft.Json;

namespace Common.Middlewares
{
    public class ExceptionErrorDetails
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
      

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
