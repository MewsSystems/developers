import {
    MOVIE_DETAILS_URL,
    MOVIE_DISCOVER_URL,
    MOVIE_SEARCH_URL,
} from '@/const/endpoints'
import { Movie } from '@/types/Movie'
import { MovieResponse } from '@/types/MovieResponse'

const options = {
    method: 'GET',
    headers: {
        accept: 'application/json',
        Authorization: `Bearer ${process.env.TMDB_API_KEY}`,
    },
}

interface FetchMoviesProps {
    page?: number
    query?: string
}

export const fetchMovies = async ({
    page = 1,
    query,
}: FetchMoviesProps): Promise<MovieResponse> => {
    const url = `${query ? MOVIE_SEARCH_URL : MOVIE_DISCOVER_URL}&query=${query}&page=${page}`
    const response = await fetch(url, options)
    const data = await response.json()
    if (!response.ok) {
        throw new Error('Failed to fetch movies')
    }
    return data
}

export const fetchMovie = async (id: string): Promise<Movie> => {
    if (!id) throw new Error('Movie ID is required')
    const url = MOVIE_DETAILS_URL(id)
    const response = await fetch(url, options)
    if (!response.ok) {
        throw new Error('Failed to fetch movie')
    }
    const data = await response.json()
    return data
}
