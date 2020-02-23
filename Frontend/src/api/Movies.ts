import axios, { AxiosPromise } from 'axios'
import { RequestConfig } from './requestConfig'
import { MovieDetail } from 'model/api/MovieDetail'
import { List } from 'model/api/List'
import { Movie } from 'model/api/Movie'

export class MoviesApi {
  /**
   * Movie Detail
   *
   * @param movie_id
   * @param append_to_response Append requests within the same namespace to the response.
   */

  static getMovieDetail(
    movie_id: number,
    append_to_response?: string,
    config?: RequestConfig
  ): AxiosPromise<MovieDetail> {
    return axios({
      ...config,
      method: 'GET',
      url: `/movie/${movie_id}`,
      params: {
        append_to_response,
      },
    })
  }

  /**
   * Trending Movies
   *
   * @param time_window
   * @param page
   */

  static getTrendingMovies(
    time_window: 'day' | 'week',
    page?: number,
    config?: RequestConfig
  ): AxiosPromise<List<Movie>> {
    return axios({
      ...config,
      method: 'GET',
      url: `/trending/movie/${time_window}`,
      params: {
        page,
      },
    })
  }
}
