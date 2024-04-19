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
        results: MovieItem[];
    }

    export interface MovieItem {
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

    export interface MovieDetail {
        adult: boolean;
        getBackdropUrl: GetBackdropImageUrl;
        belongsToCollection: string;
        budget: number;
        genres: {
            id: number;
            name: string;
        }[];
        homepage: string;
        id: string;
        imbdId: string;
        originalLanguage: string;
        originalTitle: string;
        overview: string;
        popularity: string;
        getPosterUrl: GetPosterImageUrl;
        releaseDate: Date;
        revenue: number;
        runtime: number;
        status: string;
        tagline: string;
        title: string;
        video: boolean;
        voteAverage: number;
        voteCount: number;
    }

    export interface MovieCredits {
        id: number;
        cast: CastItem[];
        crew: CrewItem[];
    }

    export interface PersonItemBase {
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

    export interface CastItem extends PersonItemBase {
        castId: number;
        character: string;
        order: number;
    }

    export interface CrewItem extends PersonItemBase {
        department: string;
        job: string;
    }
}