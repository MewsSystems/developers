import { TmdbDto } from '../interfaces/tmdbDto.ts';
import { MovieModel } from '../interfaces/movieModel.ts';
import config from '../config.json';
import { BackdropImageSize } from '../enums/images/backdropImageSize.ts';
import { PosterImageSize } from '../enums/images/posterImageSize.ts';

export namespace MovieMapper {
    // working with images https://developer.themoviedb.org/docs/image-basics
    const buildAssetUrl = <TSize>(size: TSize, assetPath: string) => {
        return assetPath ? `${config.tmdb.assetBaseUrl}${size}${assetPath}` : '';
    }

    export const movieListFromDto = (dto: TmdbDto.MovieList): MovieModel.MovieList => {
        const {
            page,
            total_pages,
            total_results
        } = dto;

        return {
            page,
            totalPages: total_pages,
            totalResults: total_results,
            results: dto.results?.map(movieItemFromDto)
        };
    };

    export const movieItemFromDto = (dto: TmdbDto.MovieItem): MovieModel.MovieItem => {
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
            getBackdropUrl: (size: BackdropImageSize) => buildAssetUrl(size, backdrop_path),
            id,
            originalLanguage: original_language,
            originalTitle: original_title,
            overview,
            popularity,
            getPosterUrl: (size: PosterImageSize) => buildAssetUrl(size, poster_path),
            title,
            video,
            voteAverage: vote_average,
            voteCount: vote_count,
        };
    };

    export const movieDetailFromDto = (dto: TmdbDto.MovieDetail): MovieModel.MovieDetail => {
        const {
            adult,
            backdrop_path,
            belongs_to_collection,
            budget,
            genres,
            homepage,
            id,
            imdb_id,
            original_language,
            original_title,
            overview,
            popularity,
            poster_path,
            release_date,
            revenue,
            runtime,
            status,
            tagline,
            title,
            video,
            vote_average,
            vote_count
        } = dto;

        return {
            adult,
            getBackdropUrl: (size: BackdropImageSize) => buildAssetUrl(size, backdrop_path),
            belongsToCollection: belongs_to_collection,
            budget,
            genres: genres.map(({ id, name }) => ({ id, name })),
            homepage,
            id,
            imbdId: imdb_id,
            originalLanguage: original_language,
            originalTitle: original_title,
            overview,
            popularity,
            getPosterUrl: (size: PosterImageSize) => buildAssetUrl(size, poster_path),
            releaseDate: new Date(Date.parse(release_date)),
            revenue,
            runtime,
            status,
            tagline,
            title,
            video,
            voteAverage: vote_average,
            voteCount: vote_count,
        }
    }
}
