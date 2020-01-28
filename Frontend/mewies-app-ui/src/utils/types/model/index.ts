import { GenreDto } from '../dto'

export interface MovieModel {
    id: string
    title: string
    overview: string
    posterPath: string
    releaseDate: string
    genres: GenreDto[]
    voteAverage: number
}
