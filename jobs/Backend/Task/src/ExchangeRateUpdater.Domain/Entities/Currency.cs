namespace Domain.Entities
{
    public class Currency
    {
        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(code));
            }

            if (code.Length != 3)
            {
                throw new ArgumentException("Currency code should only contain 3 letters.", nameof(code));
            }
            
            Code = code;
        }
        
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}
