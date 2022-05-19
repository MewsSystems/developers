namespace ExchangeRateUpdater.Domain
{
    public record ExchangeRate
    {
        public decimal Value { get; }

        private ExchangeRate(decimal value) => Value = value;

        public static Result<ExchangeRate> Create(int? ammountOrNothing, decimal? rateOrNothing)
            => rateOrNothing
                .ToResult("Currency Code is null")
                // Here we can add other validations like checking if the ammount is power of 10 
                .Ensure(_ => ammountOrNothing.HasValue && ammountOrNothing.Value >= 1m, "Ammount value incorrect")
                .Ensure(rate => rate > 0, "Rate is negative")
                .Map(rate => new ExchangeRate(rate/ammountOrNothing!.Value));

        //Ovverinding the operators can help when storing the domain into the databases to avoid avoind saving as objects and instead saving as basic type
        public static explicit operator ExchangeRate(decimal exhcangeRate) => new ExchangeRate(exhcangeRate);
        public static implicit operator decimal(ExchangeRate exchangeRate) => exchangeRate.Value;
    }

}
