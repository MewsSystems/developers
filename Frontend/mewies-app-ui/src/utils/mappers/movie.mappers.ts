import { MovieDto, MovieSearchQueryResultDto } from '../types/dto'
import { Suggestion } from '../hooks/useAutosuggest'
import { MovieModel } from '../types/model'

export const mapMovieSearchQueryResultToSuggestions = ({
    results,
}: MovieSearchQueryResultDto): Suggestion[] => {
    return results.map(result => {
        return {
            label: result.title,
            value: String(result.id),
        }
    })
}

export const mapMovieDtoToModel = (movie: MovieDto): MovieModel => {
    return {
        id: String(movie.id),
        title: movie.title,
        releaseDate: movie.release_date,
        posterPath: movie.poster_path,
        overview: movie.overview,
        genres: movie.genres,
        voteAverage: movie.vote_average,
    }
}
