import {
    MOVIE_DETAILS_URL,
    MOVIE_DISCOVER_URL,
    MOVIE_SEARCH_URL,
} from '@/const/endpoints'
import { Movie } from '@/types/Movie'

interface FetchMoviesProps {
    page?: number
    query?: string
}

export const fetchMovies = async ({
    page = 1,
    query,
}: FetchMoviesProps): Promise<Movie[]> => {
    const options = { method: 'GET', headers: { accept: 'application/json' } }

    const url = `${query ? MOVIE_SEARCH_URL : MOVIE_DISCOVER_URL}&query=${query}&page=${page}`

    const response = await fetch(url, options)
    const data = await response.json()
    return data.results
}

export const fetchMovie = async (id: string): Promise<Movie> => {
    if (!id) throw new Error('Movie ID is required')
    const options = { method: 'GET', headers: { accept: 'application/json' } }
    const url = MOVIE_DETAILS_URL(id)
    const response = await fetch(url, options)
    const data = await response.json()
    return data
}
