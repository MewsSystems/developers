import { createServerFn } from '@tanstack/start';
import { Movie } from './entities';
import * as v from 'valibot';
import { moviesMiddleware } from './middleware';
import { infiniteQueryOptions } from '@tanstack/react-query';

export interface SearchMoviesResult {
  readonly results: ReadonlyArray<Movie>;
  readonly page: number;
  readonly total_pages: number;
}

export const searchMovies = createServerFn()
  .middleware([moviesMiddleware])
  .validator(v.object({ query: v.string(), page: v.number() }))
  .handler(async ({ context, data }): Promise<SearchMoviesResult> => {
    const response = await fetch(
      `${context.moviesUrl}/search/movie?api_key=${context.apiKey}&query=${data.query}&page=${data.page}`,
    );
    return response.json();
  });

export const searchMoviesQueryOptions = (query: string) =>
  infiniteQueryOptions({
    queryKey: ['movies', 'search', query],
    queryFn: ({ pageParam }) =>
      searchMovies({ data: { query: query, page: pageParam } }),
    getNextPageParam: (lastPage) => {
      if (lastPage.page >= lastPage.total_pages) return null;

      return lastPage.page + 1;
    },
    initialPageParam: 1,
  });
