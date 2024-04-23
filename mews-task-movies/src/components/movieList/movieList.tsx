import { Movie } from '../../data/interfaces';
import MovieCard from '../movieCard/movieCard';
import './movieList.css';

export default function MovieList({
  movieValues,
  handleSelectedMovie,
}: {
  movieValues: Movie[];
  handleSelectedMovie: (movieId: number) => void;
}) {
  return (
    <div className="movie_list_container">
      {movieValues.length === 0 ? (
        <h2>No movie found (yet).</h2>
      ) : (
        movieValues.map((movie: Movie) => (
          <MovieCard
            key={movie.id}
            movie={movie}
            handleSelectedMovie={handleSelectedMovie}
          />
        ))
      )}
    </div>
  );
}
