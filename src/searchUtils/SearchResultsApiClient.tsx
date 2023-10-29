import { SearchOptionType } from '../reducers/searchReducer';
import { search } from './search';

const DEFAULT_SEARCH_TYPE = 'multi';

const COMMON_PARAMS = {
  api_key: process.env.MOVIE_SEARCH_API_KEY,
};

export class SearchResultsApiClient {
  private baseUrl: string = 'https://api.themoviedb.org/3/search/';

  public fetchMovies = (onDataReceived: any, queryParams?: any, searchType?: SearchOptionType) => {
    const apiParams = {
      ...COMMON_PARAMS,
      ...queryParams,
    };

    return this.initiateSearch(apiParams, searchType, onDataReceived);
  };

  private initiateSearch = (
    apiParams: any,
    searchType: SearchOptionType = DEFAULT_SEARCH_TYPE,
    onDataReceived: any,
  ) => {
    const abortController = new AbortController();
    search(apiParams, onDataReceived, abortController, this.baseUrl + searchType);
    return abortController;
  };
}
