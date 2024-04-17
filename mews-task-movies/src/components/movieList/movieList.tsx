import './movieList.css';
import MovieCard from '../movieCard/movieCard';
export default function MovieList({ movieValues }: { movieValues: any }) {
  return (
    <div className="movie_list_container">
      {movieValues.map((movie: any) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </div>
  );
}
