import { Movie } from '../ models/movieModel';

type MovieListProps = {
  movies: Movie[];
};

const EmptyMovieList = () => {
  return <p>No movies found</p>;
};

export const MovieList = ({ movies }: MovieListProps) => {
  return (
    <ul className='list-none flex flex-wrap items-center justify-center gap-10'>
      {movies.length === 0 && <EmptyMovieList />}
      {movies.map((movie: Movie) => {
        return (
          <li className='flex flex-col items-center max-w-56' key={movie.id}>
            <h2>{movie.title}</h2>
            <img
              className='w-60'
              src={
                movie.poster ? `https://image.tmdb.org/t/p/original/${movie.poster}` : '/src/assets/images/no-image.png'
              }
              alt={movie.title}
            />
          </li>
        );
      })}
    </ul>
  );
};
