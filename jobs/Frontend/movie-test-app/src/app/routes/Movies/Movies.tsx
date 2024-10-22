import { QueryClient, useQueryClient } from '@tanstack/react-query';
import { LoaderFunctionArgs } from 'react-router-dom';
import { getMoviesQueryOptions } from '../../api/movies.ts';

export const moviesLoader =
  (queryClient: QueryClient) =>
  async ({ request }: LoaderFunctionArgs) => {
    const url = new URL(request.url);

    const pageParam = Number(url.searchParams.get('page') || 1);
    const queryParam = url.searchParams.get('query') ?? '';

    const query = getMoviesQueryOptions({ queryParam, pageParam });

    return queryClient.getQueryData(query.queryKey) ?? (await queryClient.fetchQuery(query));
  };

export const Movies = () => {
  const queryClient = useQueryClient();
  queryClient.prefetchQuery(getMoviesQueryOptions());
  return <div>Movies</div>;
};
