import type { SimpleMovie } from "../../store/slices/movies/interfaces/simple-movie"
import MovieCard from "../movie-card/MovieCard"
import { StyledMovieList } from "./MovieList.styled"

// const movieArray = [
//   {
//     id: 1,
//     title: "Aquaman and the Lost Kingdom",
//     image: "https://image.tmdb.org/t/p/w1280/qJiWKzdRScI5OcRQqOu3qdMZKXY.jpg",
//   },
//   {
//     id: 2,
//     title: "Anyone But You",
//     image: "https://image.tmdb.org/t/p/w1280/yRt7MGBElkLQOYRvLTT1b3B1rcp.jpg",
//   },
// ]
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
