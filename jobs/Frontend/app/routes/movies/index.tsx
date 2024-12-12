import { createFileRoute, Link } from '@tanstack/react-router';
import { MovieGrid } from '../../movies/components/MovieGrid';
import { popularMoviesQueryOptions } from '../../movies/server/popular';
import { useSuspenseInfiniteQuery } from '@tanstack/react-query';
import { MovieItem } from '../../movies/components/MovieItem';
import { MoreButton } from '../../components/MoreButton';

export const PopularMovieList = () => {
  const { popularMoviesQueryOptions } = Route.useRouteContext();
  const { data, hasNextPage, fetchNextPage } = useSuspenseInfiniteQuery(
    popularMoviesQueryOptions(),
  );
  const movies = data.pages.flatMap((page) => page.results);

  return (
    <MovieGrid>
      {movies.map((movie) => (
        <MovieItem
          key={movie.id}
          movie={movie}
          renderLink={(children) => (
            <Link
              preload="intent"
              from="/movies/"
              search={(prev) => ({ ...prev, movieId: movie.id })}
            >
              {children}
            </Link>
          )}
        />
      ))}
      {hasNextPage ? <MoreButton onClick={() => fetchNextPage()} /> : null}
    </MovieGrid>
  );
};

export const Route = createFileRoute('/movies/')({
  context: () => ({
    popularMoviesQueryOptions,
  }),
  loader: async ({ context: { queryClient, popularMoviesQueryOptions } }) => {
    const queryOptions = popularMoviesQueryOptions();
    if (!queryClient.getQueryData(queryOptions.queryKey)) {
      await queryClient.fetchInfiniteQuery(queryOptions);
    }
  },
  component: PopularMovieList,
});
