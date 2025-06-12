import Movie from './Movie'

export interface MovieResponse {
    results: Movie[]
    page: number
    total_pages: number
    total_results: number
}
