import { HttpResponse, http } from "msw"
import {
  mockEmptySearchResponse,
  mockMovieDetails,
  mockPopularMoviesResponse,
  mockSearchMoviesResponse,
} from "./movieFixtures"

const BASE_URL = import.meta.env.VITE_TMDB_BASE_URL || "https://api.themoviedb.org/3"

export const handlers = [
  http.get(`${BASE_URL}/movie/popular`, () => {
    return HttpResponse.json(mockPopularMoviesResponse)
  }),

  http.get(`${BASE_URL}/search/movie`, ({ request }) => {
    const url = new URL(request.url)
    const query = url.searchParams.get("query")?.toLowerCase() || ""

    if (!query.trim()) {
      return HttpResponse.json({ status_message: "Query parameter is required" }, { status: 422 })
    }

    if (query.includes("nonexistentmovie123")) {
      return HttpResponse.json(mockEmptySearchResponse)
    }

    return HttpResponse.json(mockSearchMoviesResponse)
  }),

  http.get(`${BASE_URL}/movie/:id`, ({ params }) => {
    const { id } = params
    const movieId = Number(id)

    if (Number.isNaN(movieId) || movieId <= 0) {
      return HttpResponse.json({ status_message: "Invalid movie ID" }, { status: 400 })
    }

    if (movieId === 999999) {
      return HttpResponse.json(
        { status_message: "The resource you requested could not be found." },
        { status: 404 }
      )
    }

    return HttpResponse.json({
      ...mockMovieDetails,
      id: movieId,
    })
  }),
]
