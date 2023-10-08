namespace ExchangeRateUpdater.Application
{
    /// <summary>
    /// </summary>
    /// <param name="Code">Three-letter ISO 4217 code of the currency.</param>
    public record Currency(string Code)
    {
        public override string ToString()
        {
            return Code;
        }
    }
}
