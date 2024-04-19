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
            results: results?.map(moviePreviewFromDto)
        };
    };

    export const moviePreviewFromDto = (dto: TmdbDto.MoviePreview): MovieModel.MoviePreview => {
        return movieBaseFromDto(dto);
    };

    export const movieDetailFromDto = (dto: TmdbDto.MovieDetail): MovieModel.MovieDetail => {
        const {
            belongs_to_collection,
            budget,
            genres,
            homepage,
            imdb_id,
            release_date,
            revenue,
            runtime,
            status,
            tagline,
            ...movieBase
        } = dto;

        return {
            ...movieBaseFromDto(movieBase),
            belongsToCollection: belongs_to_collection,
            budget,
            genres: genres.map(({ id, name }) => ({ id, name })),
            homepage,
            imbdId: imdb_id,
            releaseDate: release_date ? new Date(Date.parse(release_date)) : undefined,
            revenue,
            runtime,
            status,
            tagline
        }
    }

    export const movieBaseFromDto = (dto: TmdbDto.MovieBase): MovieModel.MovieBase => {
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
    }

    export const movieCreditsFromDto = (dto: TmdbDto.MovieCredits): MovieModel.MovieCredits => {
        const {
            id,
            cast,
            crew
        } = dto;

        return {
            id,
            cast: cast?.map(castMemberFromDto),
            crew: crew?.map(crewMemberFromDto)
        }
    };

    export const castMemberFromDto = (dto: TmdbDto.CastMember): MovieModel.CastMember => {
        const {
            cast_id,
            character,
            order,
            ...personBase
        } = dto;

        return {
            ...personBaseFromDto(personBase),
            castId: cast_id,
            character,
            order
        };
    };

    export const crewMemberFromDto = (dto: TmdbDto.CrewMember): MovieModel.CrewMember => {
        const {
            department,
            job,
            ...personBase
        } = dto;


        return {
            ...personBaseFromDto(personBase),
            department,
            job
        };
    };

    export const personBaseFromDto = (dto: TmdbDto.PersonBase): MovieModel.PersonBase => {
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
