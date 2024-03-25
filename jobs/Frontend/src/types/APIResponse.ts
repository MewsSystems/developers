import { Movie } from '@/types/Movie'

export interface APIResponse {
  page: number
  results: Movie[]
  total_pages: number
  total_results: number
}
