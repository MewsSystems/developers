export type MovieDetailsType = {
    id: number
    title: string
    overview: string
    release_date: string
    poster_path: string
    vote_average: number
    genres: { id: number; name: string }[]
    runtime: number
    production_countries: { iso_3166_1: string, name: string }[]
}