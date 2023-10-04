using Domain.Exceptions;

namespace Domain.Entities
{
    public class ExchangeRateValue
    {

        public decimal Value { get; }

        public ExchangeRateValue(decimal value) 
        {
            if (value <= 0)
                throw new ExchangeRateValueMustBePositiveException(value);

            Value = value;
        }

    }
}
