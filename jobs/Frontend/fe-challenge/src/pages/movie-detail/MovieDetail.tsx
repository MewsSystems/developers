import { useParams } from 'react-router-dom';
import EmptyImageSkeleton from '@/components/EmptyImageSkeleton';
import useGetMovieDetail from '@/pages/movie-detail/hooks/useGetMovieDetail';
import MovieActors from '@/pages/movie-detail/components/MovieActors';
import MovieExtraInfo from '@/pages/movie-detail/components/MovieExtraInfo';
import MovieTitle from '@/pages/movie-detail/components/MovieTitle';
import MovieInfoSection from '@/pages/movie-detail/components/MovieInfoSection';
import IMDBLink from '@/pages/movie-detail/components/IMDBLink';

const MovieDetail = () => {
  const { movieId } = useParams();
  const { data: movie } = useGetMovieDetail(Number(movieId));

  return (
    <article>
      <section className="grid md:grid-cols-[330px_1fr] gap-10 sm:gap-12">
        <div className="max-w-[300px] md:max-w-fit aspect-[2/3] mx-auto md:mx-0">
          {movie.posterImage ? (
            <img
              className="rounded-default"
              src={movie.posterImage}
              alt={movie.title}
            />
          ) : (
            <EmptyImageSkeleton />
          )}
        </div>
        <section>
          <MovieTitle movie={movie} />
          <MovieExtraInfo movie={movie} />
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
            <MovieInfoSection title="Directed by">
              {movie.directorsFormatted}
            </MovieInfoSection>
          ) : null}
          <MovieInfoSection title="Top Actors">
            <MovieActors cast={movie.cast} />
          </MovieInfoSection>
          {movie.imdbURL ? <IMDBLink imdbURL={movie.imdbURL} /> : null}
        </section>
      </section>
    </article>
  );
};

export default MovieDetail;
