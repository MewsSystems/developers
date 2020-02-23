import axios, { AxiosPromise } from 'axios'
import { RequestConfig } from './requestConfig'
import { Credits } from 'model/api/Credits'

export class CreditsApi {
  /**
   * Movie Credits
   *
   * @param movie_id
   */

  static getMovieCredits(
    movie_id: number,
    config?: RequestConfig
  ): AxiosPromise<Credits> {
    return axios({
      ...config,
      method: 'GET',
      url: `/movie/${movie_id}/credits`,
    })
  }
}
