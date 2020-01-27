export interface MovieDto {
    adult: boolean
    backdrop_path: string
    belongs_to_collection: {
        id: number
        name: string
        poster_path: string
        backdrop_path: string
    }
    budget: number
    genres: GenreDto[]
    homepage: string
    id: 76341
    imdb_id: string
    original_language: string
    original_title: string
    overview: string
    popularity: number
    poster_path: string
    production_companies: {
        id: number
        logo_path: string
        name: string
        origin_country: string
    }
    production_countries: { iso_3166_1: string; name: string }[]
    release_date: string
    revenue: number
    runtime: number
    spoken_languages: { iso_639_1: string; name: string }[]
    status: string
    tagline: string
    title: string
    video: boolean
    vote_average: number
    vote_count: number
}

export interface GenreDto {
    id: number
    name: string
}

export interface MovieSearchQueryResultDto {
    page: number
    total_results: number
    total_pages: number
    results: MovieDto[]
}
