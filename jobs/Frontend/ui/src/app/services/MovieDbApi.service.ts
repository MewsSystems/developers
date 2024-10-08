import { trace, SpanStatusCode } from "@opentelemetry/api"

import { MovieDetail, SearchMovieResponse } from "@/app/features/movie"

import { env } from "../lib/env"

import { fetchWrapper, IClient, IService } from "."

const { MOVIE_DB_API_BASE_URL, MOVIE_DB_API_ACCESS_TOKEN } = env

const MovieDbApiClient: IClient = {
  fetch: fetchWrapper({
    baseUrl: MOVIE_DB_API_BASE_URL,
    defaultConfig: {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${MOVIE_DB_API_ACCESS_TOKEN}`,
      },
    },
  }),
}

interface IMovieDbApiService extends IService {
  searchMovies: (query: string, page: number) => Promise<SearchMovieResponse>
  getMovie: (id: string) => Promise<MovieDetail>
}

const MovieDbApiService = (): IMovieDbApiService => ({
  createUrl: (path) => {
    return `/api${path}`
  },
  async searchMovies(query, page) {
    return await trace
      .getTracer("MovieDbApiService")
      .startActiveSpan("searchMovies", async (span) => {
        try {
          span.setAttributes({
            query,
            page,
          })

          const response = await MovieDbApiClient.fetch(
            `/search/movie?query=${query}&page=${page}`,
          )
          span.setStatus({ code: SpanStatusCode.OK })

          return await response.json()
        } catch (error: any) {
          span.setStatus({
            code: SpanStatusCode.ERROR,
            message: "Unexpected error while fetching movies.",
          })
          span.recordException(error)

          throw error
        } finally {
          span.end()
        }
      })
  },
  async getMovie(id) {
    return await trace
      .getTracer("MovieDbApiService")
      .startActiveSpan("getMovie", async (span) => {
        try {
          span.setAttributes({
            id,
          })

          const response = await MovieDbApiClient.fetch(`/movie/${id}`)
          span.setStatus({ code: SpanStatusCode.OK })

          return await response.json()
        } catch (error: any) {
          span.setStatus({
            code: SpanStatusCode.ERROR,
            message: "Unexpected error while fetching movie.",
          })
          span.recordException(error)

          throw error
        } finally {
          span.end()
        }
      })
  },
})

export default MovieDbApiService()
