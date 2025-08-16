export interface MovieSearchParams {
    query: string
    page: string
}

export interface MovieSearchResultDTO {
    page: number
    total_pages: number
    total_results: number
    results: MovieDTO[]
}

export interface MovieDTO {
    adult: boolean
    backdrop_path: string
    genre_ids: number[]
    id: number
    original_language: string
    original_title: string
    overview: string
    popularity: number
    poster_path: string
    release_date: string
    title: string
    video: boolean
    vote_average: number
    vote_count: number
}

export interface MovieInformationGenreDTO {
    id: number
    name: string
}

export interface MovieInformationProductionCompanyDTO {
    id: number
    logo_path: string
    name: string
    origin_country: string
}

export interface MovieInformationProductionCountryDTO {
    iso_3166_1: string
    name: string
}

export interface MovieInformationSpokenLanguageDTO {
    english_name: string
    iso_639_1: string
    name: string
}

export interface MovieInformationDTO {
    adult: boolean
    backdrop_path: string
    belongs_to_collection: string
    budget: number
    genres: MovieInformationGenreDTO[]
    homepage: string
    id: number
    imdb_id: string
    original_language: string
    original_title: string
    overview: string
    popularity: number
    poster_path: string
    production_companies: MovieInformationProductionCompanyDTO[]
    production_countries: MovieInformationProductionCountryDTO[]
    release_date: string
    revenue: number
    runtime: number
    spoken_languages: MovieInformationSpokenLanguageDTO[]
    status: string
    tagline: string
    title: string
    video: boolean
    vote_average: number
    vote_count: number
}
