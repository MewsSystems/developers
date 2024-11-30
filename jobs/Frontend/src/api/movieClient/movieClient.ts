import BaseClient from '../baseClient/baseClient'
import { ApiError, ApiResponse, FullMovieResponse, Movie } from './types'

const apiUrl = 'https://api.themoviedb.org/3'
const apiKey: string = import.meta.env.VITE_API_KEY || ''
const defaultRequestOptions = {
  headers: {
    'Content-type': 'application/json',
  },
}
const defaultPosterUrl = 'https://image.tmdb.org/t/p/w600_and_h900_bestv2'

class MovieClient extends BaseClient {
  public defaultPosterUrl: string

  constructor(defaultPosterUrl: string) {
    super(apiUrl, apiKey, defaultRequestOptions)
    this.defaultPosterUrl = defaultPosterUrl
  }

  async getMovies(
    query: string,
    page: number = 1
  ): Promise<ApiResponse<Movie> | ApiError> {
    try {
      const response = await fetch(
        `${this.apiUrl}/search/movie?query=${query}&page=${page}&api_key=${this.apiKey}`,
        this.defaultRequestOptions
      )
      const data: ApiResponse<Movie> = await response.json()

      return data
    } catch (err) {
      console.error(err)
      return {
        message: 'An error has occurred while fetching data',
      } as ApiError
    }
  }

  async getMovieDetails(
    movieId: string
  ): Promise<FullMovieResponse | ApiError> {
    try {
      const response = await fetch(
        `${this.apiUrl}/movie/${movieId}?api_key=${this.apiKey}`,
        this.defaultRequestOptions
      )
      const data: FullMovieResponse = await response.json()
      return data
    } catch (err) {
      console.error(err)
      return {
        message: 'An error has occurred while fetching data',
      } as ApiError
    }
  }
}

export default new MovieClient(defaultPosterUrl)
