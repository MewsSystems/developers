import { MovieDTO } from './MovieDTO'

export interface MovieResponseDTO {
    results: MovieDTO[]
    page: number
    total_pages: number
    total_results: number
}
