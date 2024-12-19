import { useParams } from 'react-router';
import { useMovie } from '../hooks/useMovie';

export const MovieDetails = () => {
  const { movieId } = useParams();
  const { data: movie, isPending } = useMovie(movieId);

  if (!movie && !isPending) return <div>Not found</div>;

  return (
    <div className='w-full min-h-screen flex items-center justify-center'>
      {movie && (
        <div className='flex flex-col sm:flex-row gap-8'>
          <img
            src={movie.poster ? `https://image.tmdb.org/t/p/w300/${movie.poster}` : '/src/assets/images/no-image.png'}
            alt={movie?.title}
          />
          <div className='flex flex-col gap-4 text-start'>
            <div>
              <h1 className='m-0'>{movie.title}</h1>
              <p className='text-gray-400'>{movie.releaseDate}</p>
            </div>
            <div className='flex gap-4'>
              {movie.genres.map((genre) => (
                <span key={genre} className='border border-gray-300 rounded-full px-4 py-1'>
                  {genre}
                </span>
              ))}
            </div>
            <p>{movie.overview}</p>
          </div>
        </div>
      )}
    </div>
  );
};
