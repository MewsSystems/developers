import { useParams } from 'react-router-dom';
import EmptyImageSkeleton from '@/components/EmptyImageSkeleton';
import IMDBIcon from '@/components/IMDBIcon';
import MovieGenres from '@/pages/movie-detail/components/MovieGenres';
import useGetMovieDetail from '@/pages/movie-detail/hooks/useGetMovieDetail';
import MovieActors from '@/pages/movie-detail/components/MovieActors';

const MovieDetail = () => {
  const { movieId } = useParams();
  const { data: movie } = useGetMovieDetail(Number(movieId));

  return (
    <article>
      <section className="grid md:grid-cols-[330px_1fr] gap-12">
        <div className="aspect-[2/3]">
          {movie.posterImage ? (
            <img
              className="rounded-xl mx-auto max-w-[300px] md:max-w-full"
              src={movie.posterImage}
              alt={movie.title}
            />
          ) : (
            <EmptyImageSkeleton />
          )}
        </div>

        <section>
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
          <div className="flex">
            <span className='after:content-["·"] after:px-1'>
              {movie.country}
            </span>
            <span className='after:content-["·"] after:px-1'>
              {movie.runtime} min
            </span>
            <MovieGenres genres={movie.genres} />
          </div>
          <div className="font-semibold mt-4 mb-2">Score</div>
          <div className="text-2xl font-bold">
            {movie.voteAveragePercent} %{' '}
            <span className="font-light text-base">
              ({movie.voteCount} votes)
            </span>
          </div>
          <section className="mt-6">
            {movie.tagline && (
              <h3 className="font-semibold mb-2">{movie.tagline}</h3>
            )}
            <p>{movie.overview}</p>
          </section>
          {movie.directorsFormatted ? (
            <>
              <div className="font-semibold mt-4 mb-2">Directed by</div>
              <div>{movie.directorsFormatted}</div>
            </>
          ) : null}
          <div className="font-semibold mt-4 mb-2">Top Actors</div>
          <div>
            <MovieActors cast={movie.cast} />
          </div>
          {movie.imdbId ? (
            <a
              rel="noopener noreferrer"
              href={movie.imdbURL}
              target="_blank"
              className="bg-[#EFC200] inline-block text-sm px-2 rounded-md mt-3"
            >
              <IMDBIcon className="h-8 text-gray-600" />
            </a>
          ) : null}
        </section>
      </section>
    </article>
  );
};

export default MovieDetail;
