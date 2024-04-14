export * from './components'

export interface MovieSearchItem {
    id: number
    release_date: string
    title: string
    poster_path: string
}

interface Genre {
    id: number
    name: string
}
export interface MovieDetail extends MovieSearchItem {
    overview: string
    genres: Genre[]
    original_language: string
    runtime: number
    vote_average: number
    vote_count: number
}
