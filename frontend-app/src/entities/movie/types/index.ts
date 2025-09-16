import type { PageList } from "@/shared/api/types"

export interface Recommendation {
    adult: boolean
    backdrop_path: string
    id: number
    title: string
    original_title: string
    overview: string
    poster_path: string
    media_type: string
    original_language: string
    genre_ids: number[]
    popularity: number
    release_date: string
    video: boolean
    vote_average: number
    vote_count: number
}

export interface Images {
    backdrops: Backdrop[]
    id: number
    logos: Logo[]
    posters: Poster[]
}

export interface Backdrop {
    aspect_ratio: number
    height: number
    iso_639_1?: string
    file_path: string
    vote_average: number
    vote_count: number
    width: number
}

export interface Logo {
    aspect_ratio: number
    height: number
    iso_639_1: string
    file_path: string
    vote_average: number
    vote_count: number
    width: number
}

export interface Poster {
    aspect_ratio: number
    height: number
    iso_639_1?: string
    file_path: string
    vote_average: number
    vote_count: number
    width: number
}


export interface MovieVideo {
    iso_639_1: string
    iso_3166_1: string
    name: string
    key: string
    site: string
    size: number
    type: string
    official: boolean
    published_at: string
    id: string
}

export interface MovieReview {
    author: string
    author_details: AuthorDetails
    content: string
    created_at: string
    id: string
    updated_at: string
    url: string
}

export interface AuthorDetails {
    name: string
    username: string
    avatar_path: any
    rating: number
}

export interface MovieCollection {
    id: number;
    name: string;
    poster_path?: string;
    backdrop_path?: string;
}

export interface Collection {
    id: number
    name: string
    overview: string
    poster_path: string
    backdrop_path: string
    parts: Part[]
}

export interface Part {
    adult: boolean
    backdrop_path: string
    id: number
    title: string
    original_title: string
    overview: string
    poster_path: string
    media_type: string
    original_language: string
    genre_ids: number[]
    popularity: number
    release_date: string
    video: boolean
    vote_average: number
    vote_count: number
}


export interface MovieGenre {
    id: number;
    name: string;
}

export interface MovieProductionCountries {
    name: string;
    iso_3166_1: string;
}

export interface MovieProductionCompanies {
    id: number;
    logo_path: string;
    name: string;
    origin_country: string;
}

export interface MovieListItem {
    adult: boolean;
    backdrop_path: string | null;
    genre_ids: string[];
    id: number;
    original_language: string;
    original_title: string;
    overview: string;
    popularity: number;
    poster_path: string;
    release_date: string;
    title: string;
    video: boolean;
    vote_average: number;
    vote_count: number;
}

export interface MovieDetails {
    adult: boolean;
    backdrop_path: string | null;
    belongs_to_collection?: MovieCollection;
    budget: number;
    genres: MovieGenre[];
    homepage: string;
    id: number;
    imdb_id: string | undefined;
    origin_country: string[];
    original_language: string;
    original_title: string;
    overview: string;
    popularity: number;
    poster_path: string;
    production_companies: MovieProductionCompanies[];
    production_countries: MovieProductionCountries[];
    release_date: string;
    revenue: number;
    runtime: number;
    spoken_languages: any[];
    status: string;
    tagline: string;
    title: string;
    video: boolean;
    vote_average: number;
    vote_count: number;
}

export interface AccountStates {
    "id": number;
    "favorite": boolean;
    "rated": any;
    "watchlist": boolean;
}


export interface MovieDetailsAppend {
    keywords: { keywords: Keyword[] };
    videos: { results: MovieVideo[] };
    reviews: PageList<MovieReview>;
    recommendations: PageList<Recommendation>;
    credits: MovieCredits;
    account_states: AccountStates;
}

const selected_append_response = ['keywords', 'videos', 'reviews', 'recommendations', 'credits', 'account_states'] as const;
export const append_to_response = Object.values(selected_append_response).join(",")
export type MovieDetailsAppended = MovieDetails & Pick<MovieDetailsAppend, typeof selected_append_response[number]>;

export interface Cast {
    adult: boolean
    gender: number
    id: number
    known_for_department: string
    name: string
    original_name: string
    popularity: number
    profile_path: string
    cast_id: number
    character: string
    credit_id: string
    order: number
}

export interface Keyword {
    id: number;
    name: string;
}

export interface MovieKeyword {
    id: number;
    keywords: Keyword[]
}

export interface MovieCredits {
    cast: Cast[]
    crew: Crew[]
}

export interface Crew {
    adult: boolean
    gender: number
    id: number
    known_for_department: string
    name: string
    original_name: string
    popularity: number
    profile_path?: string
    credit_id: string
    department: string
    job: string
}


export type MovieProductionCompanyWithImg = {
    logo_img?: string;
} & MovieProductionCompanies