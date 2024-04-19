import { BackdropImageSize } from '../enums/images/backdropImageSize.ts';
import { PosterImageSize } from '../enums/images/posterImageSize.ts';
import { ProfileImageSize } from '../enums/images/profileImageSize.ts';
import { Gender } from '../enums/gender.ts';

export namespace MovieModel {
    type GetBackdropImageUrl = (size: BackdropImageSize) => string;
    type GetPosterImageUrl = (size: PosterImageSize) => string;
    type GetProfileImageUrl = (size: ProfileImageSize) => string;

    export interface MovieList {
        page: number;
        totalPages: number;
        totalResults: number;
        results: MoviePreview[];
    }

    export interface MovieBase {
        adult: boolean;
        getBackdropUrl: GetBackdropImageUrl;
        id: number;
        originalLanguage: string;
        originalTitle: string;
        overview: string;
        popularity: string;
        getPosterUrl: GetPosterImageUrl;
        title: string;
        video: boolean;
        voteAverage: number;
        voteCount: number;
    }

    export interface MoviePreview extends MovieBase { /* empty */ }

    export interface MovieDetail extends MovieBase {
        belongsToCollection: string;
        budget: number;
        genres: {
            id: number;
            name: string;
        }[];
        homepage: string;
        imbdId: string;
        releaseDate?: Date;
        revenue: number;
        runtime: number;
        status: string;
        tagline: string;
    }

    export interface MovieCredits {
        id: number;
        cast: CastMember[];
        crew: CrewMember[];
    }

    export interface PersonBase {
        adult: boolean;
        gender: Gender;
        id: number;
        knownForDepartment: string;
        name: string;
        originalName: string;
        popularity: number;
        getProfileImgUrl: GetProfileImageUrl;
        creditId: number;
    }

    export interface CastMember extends PersonBase {
        castId: number;
        character: string;
        order: number;
    }

    export interface CrewMember extends PersonBase {
        department: string;
        job: string;
    }
}