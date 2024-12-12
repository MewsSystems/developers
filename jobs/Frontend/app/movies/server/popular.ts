import { createServerFn } from '@tanstack/start';
import { Movie } from './entities';
import * as v from 'valibot';
import { moviesMiddleware } from './middleware';
import { infiniteQueryOptions } from '@tanstack/react-query';

export interface PopularMoviesResult {
  readonly results: ReadonlyArray<Movie>;
  readonly page: number;
  readonly total_pages: number;
}

export const popularMovies = createServerFn()
  .middleware([moviesMiddleware])
  .validator(v.object({ page: v.number() }))
  .handler(async ({ context, data }): Promise<PopularMoviesResult> => {
    const response = await fetch(
      `https://api.themoviedb.org/3/movie/popular?api_key=${context.apiKey}&page=${data.page}`,
    );

    return response.json();
  });

export const popularMoviesQueryOptions = () =>
  infiniteQueryOptions({
    queryKey: ['movies', 'popular'],
    queryFn: ({ pageParam }) => popularMovies({ data: { page: pageParam } }),
    getNextPageParam: (lastPage) => {
      if (lastPage.page >= lastPage.total_pages) return null;

      return lastPage.page + 1;
    },
    initialPageParam: 1,
  });
