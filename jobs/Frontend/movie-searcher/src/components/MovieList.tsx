import { Movie } from '../ models/movieModel';

type MovieListProps = {
  movies: Movie[];
};

const EmptyMovieList = () => {
  return <p>No movies found</p>;
};

export const MovieList = ({ movies }: MovieListProps) => {
  return (
    <ul className='list-none w-full grid grid-cols-[repeat(auto-fit,minmax(200px,_1fr))] gap-10'>
      {movies.length === 0 && <EmptyMovieList />}
      {movies.map((movie: Movie) => {
        return (
          <li className='grid grid-cols-subgrid' key={movie.id}>
            <h3 className='text-lg'>{movie.title}</h3>
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
