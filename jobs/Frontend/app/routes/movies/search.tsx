import { createFileRoute, Link } from '@tanstack/react-router';
import { searchMoviesQueryOptions } from '../../movies/server/search';
import { useSuspenseInfiniteQuery } from '@tanstack/react-query';
import React from 'react';
import { MovieItem } from '../../movies/components/MovieItem';
import { MoreButton } from '../../components/MoreButton';
import { MovieGrid } from '../../movies/components/MovieGrid';

export interface SearchMovieText {
  readonly query: string;
}

export const SearchMovieText = (props: SearchMovieText) => {
  const { data } = useSuspenseInfiniteQuery(
    searchMoviesQueryOptions(props.query),
  );
  const movies = data.pages.flatMap((page) => page.results);

  return (
    <div className="col-span-full row-span-1">
      {movies.length
        ? `Search results for "${props.query}"`
        : `No results for "${props.query}"`}
    </div>
  );
};

export interface SearchMovieItemsProps {
  readonly query: string;
}

export const SearchMovieItems = (props: SearchMovieItemsProps) => {
  const { searchMoviesQueryOptions } = Route.useRouteContext();
  const { data, fetchNextPage, hasNextPage } = useSuspenseInfiniteQuery(
    searchMoviesQueryOptions(props.query),
  );
  const movies = data.pages.flatMap((page) => page.results);

  return (
    <>
      {movies.map((movie) => (
        <MovieItem
          key={movie.id}
          movie={movie}
          renderLink={(children) => (
            <Link
              preload="intent"
              from="/movies/search"
              search={(prev) => ({ ...prev, movieId: movie.id })}
            >
              {children}
            </Link>
          )}
        />
      ))}
      {hasNextPage ? <MoreButton onClick={() => fetchNextPage()} /> : null}
    </>
  );
};

export const SearchMovieList = () => {
  const search = Route.useSearch();

  return (
    <>
      <React.Suspense>
        {search.query !== undefined ? (
          <SearchMovieText query={search.query} />
        ) : null}
      </React.Suspense>
      <MovieGrid>
        <React.Suspense>
          {search.query !== undefined ? (
            <SearchMovieItems query={search.query} />
          ) : null}
        </React.Suspense>
      </MovieGrid>
    </>
  );
};

export const Route = createFileRoute('/movies/search')({
  loaderDeps: ({ search }) => ({
    query: search.query,
  }),
  context: () => ({
    searchMoviesQueryOptions: searchMoviesQueryOptions,
  }),
  loader: async ({
    context: { queryClient, searchMoviesQueryOptions },
    deps: { query },
  }) => {
    if (query) {
      const queryOptions = searchMoviesQueryOptions(query);
      if (!queryClient.getQueryData(queryOptions.queryKey)) {
        await queryClient.fetchInfiniteQuery(queryOptions);
      }
    }
  },
  component: SearchMovieList,
});
