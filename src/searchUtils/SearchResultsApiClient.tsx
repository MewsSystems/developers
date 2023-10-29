import { search } from './search';

const DEFAULT_SEARCH_TYPE = 'multi';

const COMMON_PARAMS = {
  api_key: process.env.MOVIE_SEARCH_API_KEY,
};

export class SearchResultsApiClient {
  private baseUrl: string = 'https://api.themoviedb.org/3/search/';
  private searchType: string = '';
  private fullUrl: string;

  constructor(searchType = DEFAULT_SEARCH_TYPE) {
    this.searchType = searchType;
    this.fullUrl = this.baseUrl + this.searchType;
  }

  public fetchMovies = (onDataReceived: any, queryParams?: any) => {
    const apiParams = {
      ...COMMON_PARAMS,
      ...queryParams,
    };

    return this.initiateSearch(apiParams, onDataReceived);
  };

  private initiateSearch = (apiParams: any, onDataReceived: any) => {
    const abortController = new AbortController();
    search(apiParams, onDataReceived, abortController, this.fullUrl);
    return abortController;
  };
}
