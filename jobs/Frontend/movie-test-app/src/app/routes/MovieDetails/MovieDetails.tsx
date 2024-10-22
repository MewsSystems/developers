import { LoaderFunctionArgs, useLoaderData } from 'react-router-dom';
import { getMovieDetailsQueryOptions, useMovieDetails } from '../../api/movie-details.ts';

export const movieDetailsLoader =
  () =>
  async ({ params }: LoaderFunctionArgs) => {
    const movieId = params.movieId as string;

    return useMovieDetails({ movieId, queryConfig: getMovieDetailsQueryOptions(movieId) });
  };

export const MovieDetails = () => {
  const data = useLoaderData();

  return (
    <div>
      <div>{JSON.stringify(data)}</div>
    </div>
  );
};
