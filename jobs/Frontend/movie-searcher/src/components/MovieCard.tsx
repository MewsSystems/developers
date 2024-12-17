import { Movie } from '../ models/movieModel';

export const MovieCard = ({ movie }: { movie: Movie }) => {
  return (
    <div>
      <a className='text-white' href={`/movies/${movie.id}`}>
        <img
          className='w-60'
          src={movie.poster ? `https://image.tmdb.org/t/p/w300/${movie.poster}` : '/src/assets/images/no-image.png'}
          alt={movie.title}
        />
        <h3 className='text-lg'>{movie.title}</h3>
      </a>
    </div>
  );
};
