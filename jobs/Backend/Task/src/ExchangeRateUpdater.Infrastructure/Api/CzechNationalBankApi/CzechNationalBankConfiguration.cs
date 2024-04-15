namespace ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi
{
    /// <summary>
    /// Represents configuration settings for the Czech National Bank API.
    /// </summary>
    internal class CzechNationalBankConfiguration
    {
        /// <summary>
        /// Section name.
        /// </summary>
        public const string SectionName = "CzechNationalBank";

        /// <summary>
        /// Czech national bank API url.
        /// </summary>
        public string ApiUrl { get; set; } = "";
    }
}
