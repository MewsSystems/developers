import { trace, SpanStatusCode } from "@opentelemetry/api"

import { MovieDetail, SearchMovieResponse } from "@/app/features/movie"

import { fetchWrapper, IClient, IService } from "."

const NextApiClient: IClient = {
  fetch: fetchWrapper({
    baseUrl: "/api",
  }),
}

interface INextApiService extends IService {
  searchMovies: (query: string, page: number) => Promise<SearchMovieResponse>
  getMovie: (id: string) => Promise<MovieDetail>
}

const NextApiService = (): INextApiService => ({
  createUrl: (path) => {
    return `/api${path}`
  },
  async searchMovies(query, page) {
    return await trace
      .getTracer("NextApiService")
      .startActiveSpan("searchMovies", async (span) => {
        try {
          span.setAttributes({
            query,
            page,
          })
          const response = await NextApiClient.fetch(
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
      .getTracer("NextApiService")
      .startActiveSpan("getMovie", async (span) => {
        try {
          span.setAttributes({
            movieId: id,
          })
          const response = await NextApiClient.fetch(`/movie/${id}`)
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

export default NextApiService()
