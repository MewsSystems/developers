import { LoaderFunctionArgs, useLoaderData } from 'react-router-dom';
import { MovieDetailsResponse } from '../../../types/api.ts';
import { QueryClient } from '@tanstack/react-query';
import { getMovieDetailsQueryOptions } from '../../api/movie-details.ts';

export const movieDetailsLoader =
  (queryClient: QueryClient) =>
  async ({ params }: LoaderFunctionArgs) => {
    const movieId = params.movieId as string;

    const query = getMovieDetailsQueryOptions(movieId);

    return queryClient.getQueryData(query.queryKey) ?? (await queryClient.fetchQuery(query));
  };

export const MovieDetails = () => {
  const loaderData = useLoaderData() as { data: MovieDetailsResponse };
  const movieDetails = loaderData.data;

  return (
    <div>
      <div>{JSON.stringify(movieDetails)}</div>
    </div>
  );
};
