export namespace TmdbDto {
    export interface SearchMovies {
        page: number;
        total_pages: number;
        total_results: number;
        results: SearchMovieItem[];
    }

    export interface SearchMovieItem {
        adult: boolean;
        backdrop_path: string;
        genre_ids: number[];
        id: number;
        original_language: string;
        original_title: string;
        overview: string;
        popularity: string;
        poster_path: string;
        title: string;
        video: boolean;
        vote_average: number;
        vote_count: number;
    }

    // TODO: do we need to use the config endpoint?
    // https://developer.themoviedb.org/reference/configuration-details
    export interface ConfigDetails {
        images: ConfigImage[];
        change_keys: string[];
    }

    export interface ConfigImage {
        base_url: string;
        secure_base_url: string;
        backdrop_sizes: string[];
        logo_sizes: string[];
        poster_sizes: string[];
        profile_sizes: string[];
        still_sizes: string[];
    }
}

