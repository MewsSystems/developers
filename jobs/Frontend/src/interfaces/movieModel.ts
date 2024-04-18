import { BackdropImageSize } from '../enums/images/backdropImageSize.ts';
import { PosterImageSize } from '../enums/images/posterImageSize.ts';

export namespace MovieModel {
    type GetBackdropUrl = (size: BackdropImageSize) => string;
    type GetPosterUrl = (size: PosterImageSize) => string;

    export interface MovieList {
        page: number;
        totalPages: number;
        totalResults: number;
        results: MovieItem[];
    }

    export interface MovieItem {
        adult: boolean;
        getBackdropUrl: GetBackdropUrl;
        id: number;
        originalLanguage: string;
        originalTitle: string;
        overview: string;
        popularity: string;
        getPosterUrl: GetPosterUrl;
        title: string;
        video: boolean;
        voteAverage: number;
        voteCount: number;
    }

    export interface MovieDetail {
        adult: boolean;
        getBackdropUrl: GetBackdropUrl;
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
        getPosterUrl: GetPosterUrl;
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
}