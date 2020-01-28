import { getRequest } from './api.requests'
import { MovieDto, MovieSearchQueryResultDto } from '../types/dto'

export const getMoviesByQuery = (
    query: string
): Promise<MovieSearchQueryResultDto> =>
    getRequest<MovieSearchQueryResultDto>('/search/movie', {
        params: { query },
    })

export const getMovieById = (id: string): Promise<MovieDto> =>
    getRequest<MovieDto>(`/movie/${id}`)
