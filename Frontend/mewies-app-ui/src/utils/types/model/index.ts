import { GenreDto } from '../dto'

export interface MovieModel {
    title: string
    overview: string
    posterPath: string
    releaseDate: string
    genres: GenreDto[]
}
