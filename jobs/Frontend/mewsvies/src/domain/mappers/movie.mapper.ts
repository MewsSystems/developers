import {
    MovieSearchParams,
    MovieSearchResultDTO,
} from '../../api/interfaces/movie.interface.ts'
import {
    MovieSearch,
    MovieSearchResult,
} from '../interfaces/movie.interface.ts'

export const mapSearchMovieToParams = (
    data: MovieSearch
): MovieSearchParams => ({
    query: data.query,
    page: data.page,
})

export const mapMovieSearchResultFromDTO = (
    data: MovieSearchResultDTO
): MovieSearchResult => ({
    adult: data.adult,
    backdrop_path: data.backdrop_path,
    genre_ids: data.genre_ids,
    id: data.id,
    original_language: data.original_language,
    original_title: data.original_title,
    overview: data.overview,
    popularity: data.popularity,
    poster_path: data.poster_path,
    release_date: data.release_date,
    title: data.title,
    video: data.video,
    vote_average: data.vote_average,
    vote_count: data.vote_count,
})
