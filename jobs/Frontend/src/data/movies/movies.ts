import { useInfiniteQuery, useQuery } from '@tanstack/react-query'
import axios from 'axios'

export interface Genre {
  id: number
  name: string
}

export interface Company {
  id: number
  logo_path: string | null
  name: string
  origin_country: string
}

export interface Country {
  iso_3166_1: string
  name: string
}

export interface Language {
  english_name: string
  iso_639_1: string
  name: string
}

export interface Movie {
  adult: boolean
  backdrop_path: string | null
  belongs_to_collection: boolean | null
  budget: number
  genres: Genre[]
  homepage: string
  id: number
  imdb_id: string
  original_language: string
  original_title: string
  overview: string
  popularity: number
  poster_path: string | null
  production_companies: Company[]
  production_countries: Country[]
  release_date: string
  revenue: number
  runtime: number
  spoken_languages: Language[]
  status: string
  tagline: string
  title: string
  video: boolean
  vote_average: number
  vote_count: number
}

export interface MovieInfo {
  adult: boolean
  backdrop_path: string | null
  genre_ids: number[]
  id: number
  original_language: string
  original_title: string
  overview: string
  popularity: number
  poster_path: string | null
  release_date: string
  title: string
  video: boolean
  vote_average: number
  vote_count: number
}

export interface MoviePage {
  page: number
  results: MovieInfo[]
  total_pages: number
  total_results: number
}

// https://developer.themoviedb.org/reference/search-movie
export const useMovies = (query: string) => {
  return useInfiniteQuery<MoviePage>({
    queryKey: ['movies', query],
    enabled: !!query,
    initialPageParam: 1,
    getNextPageParam: (lastPage) =>
      lastPage && lastPage.page < lastPage.total_pages
        ? lastPage.page + 1
        : undefined,
    queryFn: async ({ pageParam }) => {
      // simulate slower network
      await new Promise((resolve) => setTimeout(resolve, 300))

      return axios
        .get(
          `${PUBLIC_API_URL}/3/search/movie?api_key=${PUBLIC_API_KEY}&page=${pageParam}&query=${query}`,
        )
        .then((res) => res.data)
    },
  })
}

// https://developer.themoviedb.org/reference/movie-details
export const useMovie = (movieId: string) => {
  return useQuery<Movie>({
    queryKey: ['movie', movieId],
    queryFn: async () => {
      // simulate slower network
      await new Promise((resolve) => setTimeout(resolve, 300))

      return axios
        .get(`${PUBLIC_API_URL}/3/movie/${movieId}?api_key=${PUBLIC_API_KEY}`)
        .then((res) => res.data)
    },
  })
}
