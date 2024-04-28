import { Link } from 'react-router-dom';
import EmptyImageSkeleton from '@/components/EmptyImageSkeleton';
import { Movie } from '@/modules/movies/domain/Movie';
import useBuildLocationState from '@/pages/search-tmp/hooks-tmp/useBuildLocationState';

interface MovieCardProps {
  movie: Movie;
}

const MovieCard = ({ movie }: MovieCardProps) => {
  const locationState = useBuildLocationState();

  return (
    <Link to={`movie/${movie.id}`} state={locationState} className="group">
      <div className="aspect-[2/3] transition duration-500 group-hover:scale-105 group-hover:grayscale">
        {movie.posterImage ? (
          <img
            src={movie.posterImage}
            loading="lazy"
            alt={movie.title}
            className="w-full h-auto rounded-xl aspect-[2/3]"
          />
        ) : (
          <EmptyImageSkeleton />
        )}
      </div>
      <div className="p-2">
        <h2 className="text-lg font-semibold tracking-tight line-clamp-2">
          {movie.title}
        </h2>
        {movie.releaseDateFormatted ? (
          <p className="text-gray-500">{movie.releaseDateFormatted}</p>
        ) : null}
      </div>
    </Link>
  );
};

export default MovieCard;
