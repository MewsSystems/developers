import { Movie } from '../../api/movieClient/movieClientTypes'
import MovieCard from '../movieCard/MovieCard'

type MovieListProps = {
  movies: Movie[]
}

function MovieList({ movies }: MovieListProps) {
  return movies.map((movie: Movie) => (
    <MovieCard key={movie.id} movie={movie} />
  ))
}

export default MovieList
