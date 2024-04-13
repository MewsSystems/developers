import axios from 'axios'
import { getRestApiConfig } from '../config'
import type { MovieSearchCollection, MovieSearchProps } from './types'
import type { MovieDetail } from '../../types'

export const getMovieSearchList = async ({
    movieTitle,
    page,
}: MovieSearchProps): Promise<MovieSearchCollection> => {
    const { baseUrl, headerConfig, apiKey } = getRestApiConfig()

    const endpointUrl = new URL(
        `/3/search/movie?api_key=${apiKey}&query=${movieTitle}&page=${page}`,
        baseUrl,
    ).href

    const { data } = await axios.get<MovieSearchCollection>(
        endpointUrl,
        headerConfig,
    )

    return data
}

export const getMovieDetail = async (
    movie_id: number,
): Promise<MovieDetail> => {
    const { baseUrl, headerConfig, apiKey } = getRestApiConfig()

    const endpointUrl = new URL(
        `/3/movie/${movie_id}?api_key=${apiKey}`,
        baseUrl,
    ).href

    const { data } = await axios.get<MovieDetail>(endpointUrl, headerConfig)

    return data
}
