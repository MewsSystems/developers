using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Models
{
    public class BankModel
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName = "kurzy")]
        public CoursesList Courses { get; set; }
    }

    public class CoursesList
    {

        [JsonProperty(PropertyName = "tabulka")]
        public virtual BankTable CnbTable { get; set; }

        [JsonProperty(PropertyName = "@datum")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "@created")]
        public DateTime Created
        {
            get
            {
                return DateTime.Parse(this.Date);
            }
            set
            {
                this.Date = DateTime.Now.ToLongTimeString();
            }
        }
    }

    public class BankTable
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName = "radek")]
        public virtual IList<Row> List { get; set; }
    }

    /// <summary>
    /// <radek kod="AUD" mena="dolar" mnozstvi="1" kurz="16,154" zeme="Austrálie"/>
    /// </summary>
    public class Row
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName = "@kod")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "@mena")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "@mnozstvi")]
        public int Sum { get; set; }

        [JsonProperty(PropertyName = "@kurz")]
        public decimal Course { get; set; }

        [JsonProperty(PropertyName = "@zeme")]
        public string Country { get; set; }

        public virtual BankTable CnbTable_FK { get; set; }
    }
}
