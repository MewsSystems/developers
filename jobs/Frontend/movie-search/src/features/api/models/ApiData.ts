/**
 * Interface representing the structure of a typical API response data object from TMDB.
 * @see https://developer.themoviedb.org/docs/getting-started
 */
export interface ApiData {
  /**
   * The current page number within a paginated response (if applicable).
   */
  page: number;
  /**
   * An array containing the retrieved results or data items.
   */
  results: [];
  /**
   * The total number of pages available in the entire dataset (if applicable).
   */
  total_pages: number;
  /**
   * The total number of results available in the entire dataset.
   */
  total_results: number;
}
