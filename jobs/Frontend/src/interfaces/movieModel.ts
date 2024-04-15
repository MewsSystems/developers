export namespace MovieModel {
    export interface SearchMovies {
        page: number;
        totalPages: number;
        totalResults: number;
        results: SearchMovieItem[];
    }

    export interface SearchMovieItem {
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
}