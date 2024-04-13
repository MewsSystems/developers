import axios from 'axios'
import { getRestApiConfig } from '../config'
import { MovieSearchCollection, MovieSearchProps } from './types'

export const getMovieSearchList = async ({
    movieTitle,
    page,
}: MovieSearchProps): Promise<MovieSearchCollection> => {
    const { baseUrl, headerConfig, apiKey } = getRestApiConfig()

    const endpointUrl = new URL(
        `/3/search/movie?api_key=${apiKey}&query=${movieTitle}&page=${page}`,
        baseUrl,
    ).href

    const response = await axios.get<MovieSearchCollection>(
        endpointUrl,
        headerConfig,
    )

    return response.data
}
