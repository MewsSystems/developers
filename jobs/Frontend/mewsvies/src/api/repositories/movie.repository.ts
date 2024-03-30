import { httpClient } from '../utils/httpClient.util'
import {
    MovieSearchParams,
    MovieSearchResultDTO,
} from '../interfaces/movie.interface.ts'

const URL_SEARCH = 'search/movie'

export const getMovieSearch = async (
    params: MovieSearchParams
): Promise<MovieSearchResultDTO> => {
    return httpClient.get(URL_SEARCH, {
        params,
    })
}
