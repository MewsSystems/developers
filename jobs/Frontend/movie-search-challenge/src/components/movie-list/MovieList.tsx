import type { SimpleMovie } from "../../app/slices/movieList/interfaces/simple-movie"
import MovieCard from "../movie-card/MovieCard"
import { StyledMovieList } from "./MovieList.styled"
interface Props {
  movies: SimpleMovie[]
}

const MovieList = ({ movies }: Props) => {
  return (
    <StyledMovieList>
      {movies.map(movie => (
        <MovieCard key={movie.id} {...movie} />
      ))}
    </StyledMovieList>
  )
}
export default MovieList
