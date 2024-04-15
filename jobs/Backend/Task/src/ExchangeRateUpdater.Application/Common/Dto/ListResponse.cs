namespace ExchangeRateUpdater.Application.Common.Dto
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a response containing a list of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public class ListResponse<T>
    {
        /// <summary>
        /// List of items.
        /// </summary>
        public required List<T> Data { get; set; }
    }
}
