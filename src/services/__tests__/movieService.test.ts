import { HttpResponse, http } from "msw"
import { afterEach, describe, expect, it } from "vitest"
import { ApiError } from "../../lib/api"
import {
  mockEmptySearchResponse,
  mockMovieBase,
  mockMovieDetails,
  mockPopularMoviesResponse,
  mockSearchMoviesResponse,
} from "../../test/mocks/movieFixtures"
import { server } from "../../test/mocks/server"
import { MovieServiceError, movieService } from "../movieService"

const BASE_URL = import.meta.env.VITE_TMDB_BASE_URL

afterEach(() => server.resetHandlers())

describe("movieService", () => {
  describe("getPopularMovies", () => {
    it("should fetch popular movies successfully", async () => {
      server.use(
        http.get(`${BASE_URL}/movie/popular`, () => {
          return HttpResponse.json(mockPopularMoviesResponse)
        })
      )

      const result = await movieService.getPopularMovies()
      expect(result).toMatchObject({
        page: 1,
        total_pages: 500,
        total_results: 10000,
        results: expect.arrayContaining([
          expect.objectContaining({
            id: 574475,
            title: "Final Destination Bloodlines",
          }),
        ]),
      })
    })

    it("should fetch popular movies for specific page", async () => {
      const mockResponse = {
        page: 2,
        results: [],
        total_pages: 100,
        total_results: 2000,
      }

      server.use(
        http.get(`${BASE_URL}/movie/popular`, ({ request }) => {
          const url = new URL(request.url)
          const page = url.searchParams.get("page")
          expect(page).toBe("2")
          return HttpResponse.json(mockResponse)
        })
      )

      const result = await movieService.getPopularMovies(2)
      expect(result.page).toBe(2)
    })

    it("should handle 404 error with custom message", async () => {
      server.use(
        http.get(`${BASE_URL}/movie/popular`, () => {
          return HttpResponse.json({ status_message: "Not found" }, { status: 404 })
        })
      )

      await expect(movieService.getPopularMovies()).rejects.toThrow(MovieServiceError)
      await expect(movieService.getPopularMovies()).rejects.toThrow(
        "No popular movies found for this page"
      )
    })

    it("should handle API errors", async () => {
      server.use(
        http.get(`${BASE_URL}/movie/popular`, () => {
          return HttpResponse.json({ status_message: "Server error" }, { status: 500 })
        })
      )

      await expect(movieService.getPopularMovies()).rejects.toThrow(ApiError)
    })

    it("should handle network errors", async () => {
      server.use(
        http.get(`${BASE_URL}/movie/popular`, () => {
          return HttpResponse.error()
        })
      )

      await expect(movieService.getPopularMovies()).rejects.toThrow(MovieServiceError)
      await expect(movieService.getPopularMovies()).rejects.toThrow(
        "Failed to fetch popular movies"
      )
    })
  })

  describe("searchMovies", () => {
    it("should search movies successfully", async () => {
      server.use(
        http.get(`${BASE_URL}/search/movie`, ({ request }) => {
          const url = new URL(request.url)
          const query = url.searchParams.get("query")
          expect(query).toBe("Final Destination")
          return HttpResponse.json(mockSearchMoviesResponse)
        })
      )

      const result = await movieService.searchMovies("Final Destination")
      expect(result).toMatchObject({
        page: 1,
        total_pages: 1,
        total_results: 1,
        results: expect.arrayContaining([
          expect.objectContaining({
            id: mockMovieBase.id,
            title: mockMovieBase.title,
          }),
        ]),
      })
    })

    it("should search movies with pagination", async () => {
      server.use(
        http.get(`${BASE_URL}/search/movie`, ({ request }) => {
          const url = new URL(request.url)
          const query = url.searchParams.get("query")
          const page = url.searchParams.get("page")
          expect(query).toBe("Final Destination")
          expect(page).toBe("2")
          return HttpResponse.json({
            page: 2,
            results: [],
            total_pages: 5,
            total_results: 50,
          })
        })
      )

      const result = await movieService.searchMovies("Final Destination", 2)
      expect(result.page).toBe(2)
    })

    it("should handle empty query", async () => {
      await expect(movieService.searchMovies("")).rejects.toThrow(MovieServiceError)
      await expect(movieService.searchMovies("")).rejects.toThrow("Search query cannot be empty")
    })

    it("should handle whitespace-only query", async () => {
      await expect(movieService.searchMovies("   ")).rejects.toThrow(MovieServiceError)
      await expect(movieService.searchMovies("   ")).rejects.toThrow("Search query cannot be empty")
    })

    it("should trim search query", async () => {
      server.use(
        http.get(`${BASE_URL}/search/movie`, ({ request }) => {
          const url = new URL(request.url)
          const query = url.searchParams.get("query")
          expect(query).toBe("Final Destination")
          return HttpResponse.json(mockEmptySearchResponse)
        })
      )

      await movieService.searchMovies("  Final Destination  ")
    })

    it("should handle 404 error by returning empty results", async () => {
      server.use(
        http.get(`${BASE_URL}/search/movie`, () => {
          return HttpResponse.json({ status_message: "Not found" }, { status: 404 })
        })
      )

      const result = await movieService.searchMovies("nonexistentmovie123")
      expect(result).toEqual(mockEmptySearchResponse)
    })

    it("should handle other API errors", async () => {
      server.use(
        http.get(`${BASE_URL}/search/movie`, () => {
          return HttpResponse.json({ status_message: "Server error" }, { status: 500 })
        })
      )

      await expect(movieService.searchMovies("test")).rejects.toThrow(ApiError)
    })

    it("should handle network errors", async () => {
      server.use(
        http.get(`${BASE_URL}/search/movie`, () => {
          return HttpResponse.error()
        })
      )

      await expect(movieService.searchMovies("test")).rejects.toThrow(MovieServiceError)
      await expect(movieService.searchMovies("test")).rejects.toThrow(
        'Failed to search for movies with query: "test"'
      )
    })
  })

  describe("getMovieById", () => {
    it("should fetch movie details successfully", async () => {
      server.use(
        http.get(`${BASE_URL}/movie/${mockMovieDetails.id}`, () => {
          return HttpResponse.json(mockMovieDetails)
        })
      )

      const result = await movieService.getMovieById(mockMovieDetails.id)
      expect(result).toMatchObject({
        id: mockMovieDetails.id,
        title: mockMovieDetails.title,
        runtime: mockMovieDetails.runtime,
        vote_average: mockMovieDetails.vote_average,
        vote_count: mockMovieDetails.vote_count,
        genres: expect.arrayContaining([expect.objectContaining({ name: "Horror" })]),
      })
    })

    it("should handle invalid movie ID (zero)", async () => {
      await expect(movieService.getMovieById(0)).rejects.toThrow(MovieServiceError)
      await expect(movieService.getMovieById(0)).rejects.toThrow("Invalid movie ID provided")
    })

    it("should handle invalid movie ID (negative)", async () => {
      await expect(movieService.getMovieById(-1)).rejects.toThrow(MovieServiceError)
      await expect(movieService.getMovieById(-1)).rejects.toThrow("Invalid movie ID provided")
    })

    it("should handle 404 error for non-existent movie", async () => {
      server.use(
        http.get(`${BASE_URL}/movie/999999`, () => {
          return HttpResponse.json(
            { status_message: "The resource you requested could not be found." },
            { status: 404 }
          )
        })
      )

      await expect(movieService.getMovieById(999999)).rejects.toThrow(MovieServiceError)
      await expect(movieService.getMovieById(999999)).rejects.toThrow(
        "Movie with ID 999999 not found"
      )
    })

    it("should handle other API errors", async () => {
      server.use(
        http.get(`${BASE_URL}/movie/${mockMovieDetails.id}`, () => {
          return HttpResponse.json({ status_message: "Server error" }, { status: 500 })
        })
      )

      await expect(movieService.getMovieById(mockMovieDetails.id)).rejects.toThrow(ApiError)
    })

    it("should handle network errors", async () => {
      server.use(
        http.get(`${BASE_URL}/movie/${mockMovieDetails.id}`, () => {
          return HttpResponse.error()
        })
      )

      await expect(movieService.getMovieById(mockMovieDetails.id)).rejects.toThrow(
        MovieServiceError
      )
      await expect(movieService.getMovieById(mockMovieDetails.id)).rejects.toThrow(
        `Failed to fetch movie with ID: ${mockMovieDetails.id}`
      )
    })
  })
})
