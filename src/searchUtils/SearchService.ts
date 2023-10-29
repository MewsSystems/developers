import { SearchResultsApiClient } from './SearchResultsApiClient';

export class SearchService {
  private searchResultsApiClient: any = new SearchResultsApiClient();
  private resultsUpdatedCallback: any = () => {};
  private abortController = new AbortController();

  constructor(searchType?: string) {
    this.searchResultsApiClient = new SearchResultsApiClient(searchType);
  }

  private updateResults = (results?: any) => {
    if (results === undefined) {
      return;
    }

    this.resultsUpdatedCallback(results);
  };

  public onResultsUpdate = (callback) => {
    this.resultsUpdatedCallback = callback;
    return this;
  };

  public fetchMoviesByQuery = (queryParams?: any) => {
    this.abortController.abort();
    this.abortController = this.searchResultsApiClient.fetchMovies(this.updateResults, queryParams);
    return this;
  };

  public abort = () => this.abortController.abort();
}
