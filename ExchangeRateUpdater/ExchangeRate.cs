namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        private Currency sourceCurrency_, targetCurrency_;
        private decimal value_;

        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency {
            get
            {
                return sourceCurrency_;
            }
            private set
            {
                if (value == null) throw new System.NullReferenceException("Source currency can't be null.");
                sourceCurrency_ = value;
            }
        }

        public Currency TargetCurrency {
            get
            {
                return targetCurrency_;
            }
            private set
            {
                if (value == null) throw new System.NullReferenceException("Target currency can't be null.");
                targetCurrency_ = value;
            }
        }

        public decimal Value {
            get
            {
                return value_;
            }
            private set
            {
                if (value <= 0) throw new System.ArgumentException("Value of currency can't be 0 or negative.");
                value_ = value;
            }
        }

        public override string ToString()
        {
            return SourceCurrency.Code + "/" + TargetCurrency.Code + "=" + Value;
        }
    }
}
