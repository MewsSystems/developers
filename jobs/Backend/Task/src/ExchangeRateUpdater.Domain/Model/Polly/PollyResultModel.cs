using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Model.Polly
{
    public class PollyResultModel<T>
    {
        public HttpStatusCode Code { get; set; }
        public T Response { get; set; }
    }
}
