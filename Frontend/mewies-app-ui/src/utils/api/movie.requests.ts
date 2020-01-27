import { getRequest } from './api.requests'
import { MovieSearchQueryResultDto } from '../types/dto'

export const getMoviesByQuery = (
    query: string
): Promise<MovieSearchQueryResultDto> =>
    getRequest<MovieSearchQueryResultDto>('/search/movie', {
        params: { query },
    })
