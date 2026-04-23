import { Movie } from '../ models/movieModel';
import { MovieCard } from './MovieCard';

type MovieListProps = {
  movies: Movie[];
};

const EmptyMovieList = () => {
  return <p>No movies found</p>;
};
export const MovieList = ({ movies }: MovieListProps) => {
  return movies.length ? (
    <ul className='list-none w-full grid grid-cols-[repeat(auto-fit,minmax(200px,_1fr))] gap-10'>
      {movies.map((movie: Movie) => {
        return (
          <li key={movie.id}>
            <MovieCard movie={movie} />
          </li>
        );
      })}
    </ul>
  ) : (
    <EmptyMovieList />
  );
};
