export namespace TmdbDto {
    export interface SearchMovies {
        page: number;
        total_pages: number;
        total_results: number;
        results: MovieItem[];
    }

    export interface MovieItem {
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

    export interface MovieDetail {
        adult: boolean;
        backdrop_path: string;
        belongs_to_collection: string;
        budget: number;
        genres: {
            id: number;
            name: string;
        }[];
        homepage: string;
        id: string;
        imdb_id: string;
        original_language: string;
        original_title: string;
        overview: string;
        popularity: string;
        poster_path: string;
        production_companies: {
            id: number;
            logo_path: string;
            name: string;
            origin_country: string;
        }[];
        production_countries: {
            iso_3166_1: string;
            name: string;
        }[];
        release_date: string;
        revenue: number;
        runtime: number;
        spoken_languages: {
            english_name: string;
            iso_639_1: string;
            name: string;
        }[];
        status: string;
        tagline: string;
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

