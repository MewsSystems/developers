export type SpokenLanguage = {
    english_name: string;
    iso_639_1: string;
    name: string;
};
export type Genre = { id: number; name: string };
export type Company = {
    id: number;
    name: string;
    logo_path: string | null;
    origin_country: string;
};

export type TmdbMovieDetail = {
    id: number;
    title: string;
    overview: string;
    poster_path: string | null;
    backdrop_path: string | null;
    release_date?: string;
    vote_average: number;
    vote_count: number;
    runtime?: number;
    status?: string;
    original_language?: string;
    spoken_languages?: SpokenLanguage[];
    genres?: Genre[];
    production_companies?: Company[];
    budget?: number;
    revenue?: number;
};
