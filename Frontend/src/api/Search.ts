import axios, { AxiosPromise } from 'axios'
import { RequestConfig } from './requestConfig'
import { List } from 'model/api/List'
import { Movie } from 'model/api/Movie'

export class SearchApi {
  /**
   * Search
   *
   * @param query Pass a text query to search. This value should be URI encoded.
   * @param page Specify which page to query.
   * @param include_adult Choose whether to inlcude adult (pornography) content in the results.
   * @param region Specify a ISO 3166-1 code to filter release dates. Must be uppercase.
   * @param year
   * @param primary_release_year
   * @param config The AxiosRequestConfig properties not predefined by the generated API
   */

  static getSearchResults(
    query: string,
    page?: number,
    include_adult?: boolean,
    region?: string,
    year?: number,
    primary_release_year?: number,
    config?: RequestConfig
  ): AxiosPromise<List<Movie>> {
    return axios({
      ...config,
      method: 'GET',
      url: `/search/movies`,
      params: {
        query,
        page,
        include_adult,
        region,
        year,
        primary_release_year,
      },
    })
  }
}
