import {useQuery} from '@tanstack/react-query';
import {Link, useParams, useSearchParams} from 'react-router-dom';
import {fetchMovieDetails} from '../../api/fetchMovieDetails.ts';

export default function MovieDetailsPage() {
  const {id} = useParams<{id: string}>();
  const [searchParams] = useSearchParams();

  // Create a URL with the current search params for the back link
  const backToSearchUrl = `/?${searchParams.toString()}`;

  const {data: movie, isLoading} = useQuery({
    queryKey: ['movie', id],
    queryFn: () => fetchMovieDetails(id!),
    enabled: Boolean(id),
  });

  // TODO create a LoadingSkeleton for loading state
  if (isLoading) {
    return <div>LOADING MOVIE DETAILS</div>;
  }

  // TODO create a MovieNotFound component
  if (!movie) {
    return <div>Movie not found</div>;
  }

  return (
    <div>
      <Link to={backToSearchUrl}>Back to search</Link>
      <h1>{movie.title}</h1>
      <p>{movie.overview}</p>
      <p>Release date: {movie.release_date}</p>
      <p>Rating: {movie.vote_average}</p>
      {movie.poster_path && (
        <img src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`} alt={movie.title} />
      )}
    </div>
  );
}

MovieDetailsPage.displayName = 'MovieDetailsPage';
