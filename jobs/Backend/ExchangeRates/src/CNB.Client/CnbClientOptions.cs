namespace CNB.Client
{
    public record CnbClientOptions
    {
        /// <summary>
        /// Host url (without version).
        /// </summary>
        public Uri? BaseUrl { get; set; }
    }
}
