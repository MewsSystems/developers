using Infrastructure.Entities.Xml;
using System;

namespace Infrastructure.Entities
{
    public class GenericRate
    {
        public string Code { get; private set; }
        public decimal Amount { get; private set; }
        public decimal CurrentRate { get; private set; }

        public GenericRate(RadekModel rate)
        {
            Code = rate.Code;
            Amount = Convert.ToDecimal(rate.Amount);
            CurrentRate = Convert.ToDecimal(rate.CurrentRate);
        }

        public GenericRate(string code, decimal rate)
        {
            Code = code;
            Amount = 1;
            CurrentRate = rate;
        }
    }
}
