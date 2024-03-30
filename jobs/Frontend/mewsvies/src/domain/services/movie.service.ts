import * as repository from '../../api/repositories/movie.repository.ts'
import {
    MovieSearch,
    MovieSearchResult,
} from '../interfaces/movie.interface.ts'
import {
    mapMovieSearchResultFromDTO,
    mapSearchMovieToParams,
} from '../mappers/movie.mapper.ts'

export const getMovieSearch = async (
    params: MovieSearch
): Promise<MovieSearchResult> => {
    const data = await repository.getMovieSearch(mapSearchMovieToParams(params))
    return mapMovieSearchResultFromDTO(data)
}
