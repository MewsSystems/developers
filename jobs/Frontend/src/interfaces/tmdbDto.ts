export namespace TmdbDto {
    export interface MovieList {
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

    export interface MovieCredits {
        id: number;
        cast: CastItem[];
        crew: CrewItem[];
    }

    export interface PersonItemBase {
        adult: boolean;
        gender: Gender;
        id: number;
        known_for_department: string;
        name: string;
        original_name: string;
        popularity: number;
        profile_path: string;
        credit_id: number;
    }

    export interface CastItem extends PersonItemBase {
        cast_id: number;
        character: string;
        order: number;
    }

    export interface CrewItem extends PersonItemBase {
        department: string;
        job: string;
    }

    // https://developer.themoviedb.org/reference/person-details#genders
    export const enum Gender {
        NotSpecified = 0,
        Female = 1,
        Male = 2,
        NonBinary = 3
    }
}

