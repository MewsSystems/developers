export namespace MovieModel {
    export interface SearchMovies {
        page: number;
        totalPages: number;
        totalResults: number;
        results: MovieItem[];
    }

    export interface MovieItem {
        adult: boolean;
        backdropUrl: string;
        id: number;
        originalLanguage: string;
        originalTitle: string;
        overview: string;
        popularity: string;
        posterUrl: string;
        title: string;
        video: boolean;
        voteAverage: number;
        voteCount: number;
    }

    export interface MovieDetail {
        adult: boolean;
        backdropUrl: string;
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
        posterUrl: string;
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