import { Movie } from '../ models/movieModel';

export const MovieCard = ({ movie }: { movie: Movie }) => {
  return (
    <div data-testId='movieCard' className='h-full'>
      <a
        href={`/movies/${movie.id}`}
        className='flex flex-col items-center h-full ease-in-out duration-500 hover:scale-110'
      >
        <img
          className='h-full w-64 object-cover'
          src={movie.poster ? `https://image.tmdb.org/t/p/w300/${movie.poster}` : '/src/assets/images/no-image.jpg'}
          alt={movie.title}
        />
        <h3 className='text-lg dark:text-white text-black'>{movie.title}</h3>
      </a>
    </div>
  );
};
