import ImageIcon from '@/components/ImageIcon';
import { Movie } from '@/modules/movies/domain/Movie';
import { Link } from 'react-router-dom';

interface MovieCardProps {
  movie: Movie;
}

const MovieCard = ({ movie }: MovieCardProps) => {
  return (
    <Link to={`movie/${movie.id}`} className="group">
      <div className="aspect-[2/3] transition duration-500 group-hover:scale-105 group-hover:grayscale">
        {movie.posterImage ? (
          <img
            src={movie.posterImage}
            loading="lazy"
            alt={movie.title}
            className="w-full h-auto rounded-xl aspect-[2/3]"
          />
        ) : (
          <div className="flex h-full rounded-xl items-center justify-center bg-gray-100">
            <ImageIcon className="w-9 h-9 text-gray-300" />
          </div>
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
