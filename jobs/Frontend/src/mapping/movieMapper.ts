import { TmdbDto } from '../interfaces/tmdbDto.ts';
import { MovieModel } from '../interfaces/movieModel.ts';
import config from '../config.json';
import { BackdropImageSize } from '../enums/images/backdropImageSize.ts';
import { PosterImageSize } from '../enums/images/posterImageSize.ts';
import { Gender } from '../enums/gender.ts';
import { ProfileImageSize } from '../enums/images/profileImageSize.ts';

export namespace MovieMapper {
    // working with images https://developer.themoviedb.org/docs/image-basics
    const buildAssetUrl = <TSize>(size: TSize, assetPath: string) => {
        return assetPath ? `${config.tmdb.assetBaseUrl}${size}${assetPath}` : '';
    }

    export const movieListFromDto = (dto: TmdbDto.MovieList): MovieModel.MovieList => {
        const {
            page,
            total_pages,
            total_results,
            results
        } = dto;

        return {
            page,
            totalPages: total_pages,
            totalResults: total_results,
            results: results?.map(movieItemFromDto)
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

    export const movieCreditsFromDto = (dto: TmdbDto.MovieCredits): MovieModel.MovieCredits => {
        const {
            id,
            cast,
            crew
        } = dto;

        return {
            id,
            cast: cast?.map(castItemFromDto),
            crew: crew?.map(crewItemFromDto)
        }
    };

    export const castItemFromDto = (dto: TmdbDto.CastItem): MovieModel.CastItem => {
        const {
            cast_id,
            character,
            order,
            ...personBase
        } = dto;

        return {
            ...personItemBaseFromDto(personBase),
            castId: cast_id,
            character,
            order
        };
    };

    export const crewItemFromDto = (dto: TmdbDto.CrewItem): MovieModel.CrewItem => {
        const {
            department,
            job,
            ...personBase
        } = dto;


        return {
            ...personItemBaseFromDto(personBase),
            department,
            job
        };
    };

    export const personItemBaseFromDto = (dto: TmdbDto.PersonItemBase): MovieModel.PersonItemBase => {
        const {
            adult,
            gender,
            id,
            known_for_department,
            name,
            original_name,
            popularity,
            profile_path,
            credit_id
        } = dto;

        return {
            adult,
            gender: genderFromDto(gender),
            id,
            knownForDepartment: known_for_department,
            name,
            originalName: original_name,
            popularity,
            getProfileImgUrl: (size: ProfileImageSize) => buildAssetUrl(size, profile_path),
            creditId: credit_id
        };
    };

    const genderFromDto = (dtoGender: TmdbDto.Gender): Gender => {
        return ({
            [TmdbDto.Gender.NotSpecified]: Gender.NotSpecified,
            [TmdbDto.Gender.Female]: Gender.Female,
            [TmdbDto.Gender.Male]: Gender.Male,
            [TmdbDto.Gender.NonBinary]: Gender.NonBinary
        })[dtoGender] || Gender.NotSpecified;
    }
}
