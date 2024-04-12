import axios from 'axios'
import { getRestApiConfig } from '../config'

export const getMovieSearchList = async (movieTitle: string) => {
    const { baseUrl, headerConfig, apiKey } = getRestApiConfig()

    const endpointUrl = new URL(
        `/3/search/movie?api_key=${apiKey}&query=${movieTitle}`,
        baseUrl,
    ).href

    const response = await axios.get(endpointUrl, headerConfig)

    return response.data
}
