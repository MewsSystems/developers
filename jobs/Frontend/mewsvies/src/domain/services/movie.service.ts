import * as repository from '../../api/repositories/movie.repository.ts'
import {
    MovieSearch,
    MovieSearchResult,
    MovieInformation,
} from '../interfaces/movie.interface.ts'
import {
    mapMovieSearchResultFromDTO,
    mapSearchMovieToParams,
    mapMovieInformationFromDTO,
} from '../mappers/movie.mapper.ts'

export const getMovieSearch = async (
    params: MovieSearch
): Promise<MovieSearchResult> => {
    const data = await repository.getMovieSearch(mapSearchMovieToParams(params))
    return mapMovieSearchResultFromDTO(data)
}

export const getMovieDetail = async (id: string): Promise<MovieInformation> => {
    const data = await repository.getMovieDetail(id)
    return mapMovieInformationFromDTO(data)
}
