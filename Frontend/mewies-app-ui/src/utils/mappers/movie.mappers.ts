import { MovieDto, MovieSearchQueryResultDto } from '../types/dto'
import { Suggestion } from '../hooks/useAutosuggest'
import { MovieModel } from '../types/model'

export const mapMovieSearchQueryResultToSuggestions = ({
    results,
}: MovieSearchQueryResultDto): Suggestion[] => {
    return results.map(result => {
        return {
            label: result.title,
            value: result.title,
        }
    })
}

export const mapMovieDtoToModel = (movie: MovieDto): MovieModel => {
    return {
        title: movie.title,
        releaseDate: movie.release_date,
        posterPath: movie.poster_path,
        overview: movie.overview,
        genres: movie.genres,
    }
}
