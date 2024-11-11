import { SearchOptionType } from '../reducers/searchReducer';
import { SearchResponse } from '../types';
import { search } from './search';

const DEFAULT_SEARCH_TYPE = 'multi';

const COMMON_PARAMS = {
  api_key: process.env.MOVIE_SEARCH_API_KEY,
};

export class SearchResultsApiClient {
  private baseUrl: string = 'https://api.themoviedb.org/3/search/';

  public fetchMovies = (
    onDataReceived: (data: SearchResponse) => void,
    queryParams?: Record<string, string>,
    searchType?: SearchOptionType,
  ) => {
    const apiParams = {
      ...COMMON_PARAMS,
      ...queryParams,
    };

    return this.initiateSearch(apiParams, searchType, onDataReceived);
  };

  private initiateSearch = (
    apiParams: Record<string, string>,
    searchType: SearchOptionType = DEFAULT_SEARCH_TYPE,
    onDataReceived: (data: SearchResponse) => void,
  ) => {
    const abortController = new AbortController();
    search(apiParams, onDataReceived, abortController, this.baseUrl + searchType);
    return abortController;
  };
}
