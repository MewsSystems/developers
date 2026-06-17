import { MovieDetail } from '@/modules/movies/domain/MovieDetail';
import MovieGenres from '@/pages/movie-detail/components/MovieGenres';

interface MovieExtraInfoProps {
  movie: MovieDetail;
}

const MovieExtraInfo = ({ movie }: MovieExtraInfoProps) => {
  return (
    <div className="flex flex-wrap">
      <span className='after:content-["·"] after:px-1'>{movie.country}</span>
      <span className='after:content-["·"] after:px-1'>
        {movie.runtime} min
      </span>
      <MovieGenres genres={movie.genres} />
    </div>
  );
};
export default MovieExtraInfo;
