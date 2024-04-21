import './movieList.css';
import MovieCard from '../movieCard/movieCard';

export default function MovieList({
  movieValues,
  handleSelectedMovie,
}: {
  movieValues: unknown[];
  handleSelectedMovie: (movieId: number) => void;
}) {
  return (
    <div className="movie_list_container">
      {movieValues.length === 0 && <h2>No movie found (yet).</h2>}
      {movieValues.map((movie: any) => (
        <MovieCard
          key={movie.id}
          movie={movie}
          handleSelectedMovie={handleSelectedMovie}
        />
      ))}
    </div>
  );
}
