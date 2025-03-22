import { useParams, Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { fetchMovieDetails } from '../search-api';
import { PageSection } from '../components/PageSection';
import fallback_image from './../assets/image-load-failed.svg';

// console.log(fetchMovieDetails(50));
interface Genre {
  id: number;
  name: string;
}

export const MovieDetail = () => {
  const { movieId } = useParams<{ movieId: string }>();

  const numericMovieId = Number(movieId);

  const {
    data: movie,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ['movieDetails', numericMovieId],
    queryFn: () => fetchMovieDetails(numericMovieId),
    enabled: !!movieId, // Don't run the query if no movieId
  });

  if (isLoading) return <div>Loading movie details...</div>;
  if (isError) return <div>Error loading movie details!</div>;

  console.log(fetchMovieDetails(671));
  return (
    <>
      <PageSection>
        <Link to="/">⬅️ Back to search</Link>
      </PageSection>
      <PageSection>
        <img
          src={
            movie.poster_path
              ? `https://image.tmdb.org/t/p/w300/${movie.poster_path}`
              : fallback_image
          }
          alt="Movie poster"
        />
        <div>
          <h1>
            {movie.title} ({movie.release_date})
          </h1>
          <p>
            {movie?.genres.map((genre: Genre) => {
              return <span key={genre.name}>{genre.name}</span>;
            })}
          </p>
        </div>
        <div>
          <p>{movie.vote_average}</p>
          <p>{`${movie.runtime} min`}</p>
        </div>
        <div>
          <p>{movie.tagline}</p>
          <h2>Overview</h2>
          <p>{movie.overview}</p>
        </div>
      </PageSection>
    </>
  );
};
