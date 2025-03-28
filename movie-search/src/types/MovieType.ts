export type MovieType = {
    id: number
    title: string
    poster_path: string | null
    release_date: string | null
    overview: string
    vote_average: number
    genre_ids: number[]
    backdrop_path: string | null
    original_title: string
    popularity: number
    adult: boolean
    original_language: string
    vote_count: number
    video: boolean
}