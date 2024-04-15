import { TmdbDto } from '../interfaces/tmdbDto.ts';
import { MovieModel } from '../interfaces/movieModel.ts';

export namespace MovieMapper {
    // TODO: add enums for supported image sizes? - https://developer.themoviedb.org/reference/configuration-details
    // TODO: move to config
    const assetBaseUrl = 'https://image.tmdb.org/t/p/';

    export const searchFromDto = (dto: TmdbDto.SearchMovies): MovieModel.SearchMovies => {
        const {
            page,
            total_pages,
            total_results
        } = dto;

        return {
            page,
            totalPages: total_pages,
            totalResults: total_results,
            results: dto.results?.map(itemFromDto)
        };
    };

    export const itemFromDto = (dto: TmdbDto.SearchMovieItem): MovieModel.SearchMovieItem => {
        const {
            adult,
            backdrop_path,
            id,
            original_language,
            original_title,
            overview,
            popularity,
            poster_path,
            title,
            video,
            vote_average,
            vote_count
        } = dto;

        return {
            adult,
            backdropUrl: `${assetBaseUrl}w1280${backdrop_path}`,
            id,
            originalLanguage: original_language,
            originalTitle: original_title,
            overview,
            popularity,
            posterUrl: `${assetBaseUrl}w780${poster_path}`,
            title,
            video,
            voteAverage: vote_average,
            voteCount: vote_count,
        };
    };
}
