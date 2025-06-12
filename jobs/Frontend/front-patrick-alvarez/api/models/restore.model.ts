import Movie from '@/types/Movie'
import { MovieResponse } from '@/types/MovieResponse'
import { MovieDTO } from '../types/MovieDTO'
import { MovieResponseDTO } from '../types/MovieResponseDTO'

export function restoreMovie(dto: MovieDTO): Movie {
    return new Movie({
        id: dto.id,
        title: dto.title,
        overview: dto.overview,
        releaseDate: dto.release_date,
        voteAverage: dto.vote_average,
        posterPath: dto.poster_path,
        backdropPath: dto.backdrop_path,
        originalLanguage: dto.original_language,
        tagline: dto.tagline,
        popularity: dto.popularity,
    })
}

export function restoreMovieResponse(dto: MovieResponseDTO): MovieResponse {
    return {
        results: dto.results.map(restoreMovie),
        page: dto.page,
        total_pages: dto.total_pages,
        total_results: dto.total_results,
    }
}
