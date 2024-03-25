import { Movie } from '@/types/Movie'

export interface SearchMoviesAPIResponse {
  page: number
  results: Movie[]
  total_pages: number
  total_results: number
}
