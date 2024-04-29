import { MovieDetail } from '@/modules/movies/domain/MovieDetail';

interface MovieTitleProps {
  movie: MovieDetail;
}

const MovieTitle = ({ movie }: MovieTitleProps) => {
  return (
    <>
      <h1 className="text-2xl font-bold">
        {movie.title}{' '}
        {movie.releaseYear ? (
          <span className="font-light text-gray-600">
            ({movie.releaseYear})
          </span>
        ) : null}
      </h1>
      {movie.originalTitle ? (
        <h2 className="text-lg italic mb-1">{movie.originalTitle}</h2>
      ) : null}
    </>
  );
};

export default MovieTitle;
