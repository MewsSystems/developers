import type { Movie, MovieListResponse, MovieSearchResponse } from "../types/movie"
import { ApiError, api } from "./api"

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
        if (error.code === "NOT_FOUND") {
          throw new MovieServiceError("No popular movies found for this page", error)
        }
        throw error
      }

      throw new MovieServiceError("Failed to fetch popular movies", error as Error)
    }
  },

  searchMovies: async (query: string, page = 1): Promise<MovieSearchResponse> => {
    try {
      if (!query || query.trim().length === 0) {
        throw new MovieServiceError("Search query cannot be empty")
      }

      const response = await api.get<MovieSearchResponse>("/search/movie", {
        params: { query: query.trim(), page },
      })
      return response.data
    } catch (error) {
      if (error instanceof ApiError) {
        if (error.code === "NOT_FOUND") {
          return {
            page: 1,
            results: [],
            total_pages: 0,
            total_results: 0,
          }
        }
        throw error
      }

      if (error instanceof MovieServiceError) {
        throw error
      }

      throw new MovieServiceError(
        `Failed to search for movies with query: "${query}"`,
        error as Error
      )
    }
  },

  getMovieById: async (id: number): Promise<Movie> => {
    try {
      if (!id || id <= 0) {
        throw new MovieServiceError("Invalid movie ID provided")
      }

      const response = await api.get<Movie>(`/movie/${id}`)
      return response.data
    } catch (error) {
      if (error instanceof ApiError) {
        if (error.code === "NOT_FOUND") {
          throw new MovieServiceError(`Movie with ID ${id} not found`, error)
        }
        throw error
      }

      if (error instanceof MovieServiceError) {
        throw error
      }

      throw new MovieServiceError(`Failed to fetch movie with ID: ${id}`, error as Error)
    }
  },
}
