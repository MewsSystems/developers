import { httpClient } from '../utils/httpClient.util'
import {
    MovieInformationDTO,
    MovieSearchParams,
    MovieSearchResultDTO,
} from '../interfaces/movie.interface.ts'

const URL_SEARCH = 'search/movie'
const URL_MOVIE_DETAIL = 'movie/:id'

export const getMovieSearch = async (
    params: MovieSearchParams
): Promise<MovieSearchResultDTO> => {
    const { data } = await httpClient.get(URL_SEARCH, {
        params,
    })

    return data
}

export const getMovieDetail = async (
    id: string
): Promise<MovieInformationDTO> => {
    const { data } = await httpClient.get(
        URL_MOVIE_DETAIL.replace(':id', `${id}`)
    )

    return data
}
