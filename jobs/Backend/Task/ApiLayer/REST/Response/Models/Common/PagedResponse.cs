namespace REST.Response.Models.Common;

/// <summary>
/// Paginated API response for large data sets.
/// </summary>
/// <typeparam name="T">The type of items in the collection</typeparam>
public class PagedResponse<T> : ApiResponse<IEnumerable<T>>
{
    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Indicates if there is a previous page.
    /// </summary>
    public bool HasPrevious { get; set; }

    /// <summary>
    /// Indicates if there is a next page.
    /// </summary>
    public bool HasNext { get; set; }

    /// <summary>
    /// Creates a successful paginated response.
    /// </summary>
    public static PagedResponse<T> Ok(
        IEnumerable<T> items,
        int pageNumber,
        int pageSize,
        int totalCount,
        string? message = null)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResponse<T>
        {
            Success = true,
            Data = items,
            Message = message,
            StatusCode = 200,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPrevious = pageNumber > 1,
            HasNext = pageNumber < totalPages
        };
    }

    /// <summary>
    /// Creates an empty paginated response.
    /// </summary>
    public static PagedResponse<T> Empty(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(Enumerable.Empty<T>(), pageNumber, pageSize, 0, "No items found");
    }
}
