import { ERROR_CODES, ERROR_MESSAGES } from "@/constants/errors"
import { ApiError, api } from "@/lib/api"
import type { MovieDetails, MovieListResponse, MovieSearchResponse } from "@/types/movie"

export class MovieServiceError extends Error {
  originalError?: Error

  constructor(message: string, originalError?: Error) {
    super(message)
    this.name = "MovieServiceError"
    this.originalError = originalError
  }
}

export const movieService = {
  getPopularMovies: async (page = 1): Promise<MovieListResponse> => {
    try {
      const response = await api.get<MovieListResponse>("/movie/popular", {
        params: { page },
      })
      return response.data
    } catch (error) {
      if (error instanceof ApiError) {
        if (error.code === ERROR_CODES.NOT_FOUND) {
          throw new MovieServiceError(ERROR_MESSAGES.POPULAR_MOVIES_NOT_FOUND, error)
        }
        if (error.code === ERROR_CODES.NETWORK_ERROR) {
          throw new MovieServiceError(ERROR_MESSAGES.POPULAR_MOVIES_FETCH_FAILED, error)
        }
        throw error
      }

      throw new MovieServiceError(ERROR_MESSAGES.POPULAR_MOVIES_FETCH_FAILED, error as Error)
    }
  },

  searchMovies: async (query: string, page = 1): Promise<MovieSearchResponse> => {
    try {
      if (!query || query.trim().length === 0) {
        throw new MovieServiceError(ERROR_MESSAGES.SEARCH_QUERY_EMPTY)
      }

      const response = await api.get<MovieSearchResponse>("/search/movie", {
        params: { query: query.trim(), page },
      })
      return response.data
    } catch (error) {
      if (error instanceof ApiError) {
        if (error.code === ERROR_CODES.NOT_FOUND) {
          return {
            page: 1,
            results: [],
            total_pages: 0,
            total_results: 0,
          }
        }
        if (error.code === ERROR_CODES.NETWORK_ERROR) {
          throw new MovieServiceError(ERROR_MESSAGES.MOVIE_SEARCH_FAILED(query), error)
        }
        throw error
      }

      if (error instanceof MovieServiceError) {
        throw error
      }

      throw new MovieServiceError(ERROR_MESSAGES.MOVIE_SEARCH_FAILED(query), error as Error)
    }
  },

  getMovieById: async (id: number): Promise<MovieDetails> => {
    try {
      if (!id || id <= 0) {
        throw new MovieServiceError(ERROR_MESSAGES.INVALID_MOVIE_ID)
      }

      const response = await api.get<MovieDetails>(`/movie/${id}`)
      return response.data
    } catch (error) {
      if (error instanceof ApiError) {
        if (error.code === ERROR_CODES.NOT_FOUND) {
          throw new MovieServiceError(ERROR_MESSAGES.MOVIE_NOT_FOUND(id), error)
        }
        if (error.code === ERROR_CODES.NETWORK_ERROR) {
          throw new MovieServiceError(ERROR_MESSAGES.MOVIE_FETCH_FAILED(id), error)
        }
        throw error
      }

      if (error instanceof MovieServiceError) {
        throw error
      }

      throw new MovieServiceError(ERROR_MESSAGES.MOVIE_FETCH_FAILED(id), error as Error)
    }
  },
}
