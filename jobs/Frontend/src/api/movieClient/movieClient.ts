import BaseClient from '../baseClient/baseClient'
import {
  ApiError,
  ApiResponse,
  FullMovieResponse,
  Movie,
} from './movieClientTypes'

const apiUrl = 'https://api.themoviedb.org/3'
const apiKey: string =
  import.meta.env.VITE_API_KEY || '03b8572954325680265531140190fd2a'
const defaultRequestOptions = {
  headers: {
    'Content-type': 'application/json',
  },
}
const basePosterUrl = 'https://image.tmdb.org/t/p/w600_and_h900_bestv2'
const placeHolderPosterUrl =
  'https://critics.io/img/movies/poster-placeholder.png'

class MovieClient extends BaseClient {
  public basePosterUrl: string

  constructor(basePosterUrl: string) {
    super(apiUrl, apiKey, defaultRequestOptions)
    this.basePosterUrl = basePosterUrl
  }

  buildMoviePosterUrl(relativeUrl: string): string {
    if (!relativeUrl) return placeHolderPosterUrl
    return `${this.basePosterUrl}${relativeUrl}`
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
    } catch (error) {
      console.error(error)
      return {
        message: 'An error has occurred while fetching movies',
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
    } catch (error) {
      console.error(error)
      return {
        message: `An error has occurred while fetching movie details with movieId: ${movieId}`,
      } as ApiError
    }
  }

  async getNowPlayingMovies(): Promise<ApiResponse<Movie> | ApiError> {
    try {
      const response = await fetch(
        `${this.apiUrl}/movie/now_playing?api_key=${this.apiKey}`,
        this.defaultRequestOptions
      )
      const data: ApiResponse<Movie> = await response.json()
      return data
    } catch (error) {
      console.error(error)
      return {
        message: 'An error has occurred while fetching now playing movies',
      } as ApiError
    }
  }

  async getUpcomingMovies(): Promise<ApiResponse<Movie> | ApiError> {
    try {
      const response = await fetch(
        `${this.apiUrl}/movie/upcoming?api_key=${this.apiKey}`,
        {
          headers: {
            'Content-type': 'application/json',
          },
        }
      )
      const data: ApiResponse<Movie> = await response.json()
      return data
    } catch (error) {
      console.error(error)
      return {
        message: 'An error has occurred while fetching upcoming movies',
      } as ApiError
    }
  }
}

export default new MovieClient(basePosterUrl)
