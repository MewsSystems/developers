import { SearchOptionType } from '../reducers/searchReducer';
import { SearchResponse } from '../types';
import { SearchResultsApiClient } from './SearchResultsApiClient';

export class SearchService {
  private searchResultsApiClient: any = new SearchResultsApiClient();
  private resultsUpdatedCallback: any = () => {};
  private abortController = new AbortController();

  constructor() {
    this.searchResultsApiClient = new SearchResultsApiClient();
  }

  private updateResults = (results?: SearchResponse) => {
    if (results === undefined) {
      return;
    }

    this.resultsUpdatedCallback(results);
  };

  public onResultsUpdate = (callback: () => void) => {
    this.resultsUpdatedCallback = callback;
    return this;
  };

  public fetchMoviesByQuery = (
    queryParams?: Record<string, string>,
    searchType?: SearchOptionType,
  ) => {
    this.abortController.abort();
    this.abortController = this.searchResultsApiClient.fetchMovies(
      this.updateResults,
      queryParams,
      searchType,
    );
    return this;
  };

  public abort = () => this.abortController.abort();
}
