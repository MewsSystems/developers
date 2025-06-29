import { useRef } from "react";
import { Grid } from "../../components/Grid/Grid";
import { MovieCard } from "./MovieCard";
import { usePopularMoviesQuery } from "../../hooks/useMoviesQueries";
import { useSearchMoviesQuery } from "../../hooks/useSearchMoviesQuery";
import { useInfiniteScroll } from "../../hooks/useInfiniteScroll";
import {
  GridFooterMessageWrapper,
  GridStatusMessage,
  Text,
} from "../../components/Grid/Grid.internal";
interface MoviesListProps {
  debouncedTerm: string;
}

export const MoviesList = (props: MoviesListProps) => {
  const loadMoreRef = useRef<HTMLDivElement | null>(null);

  const popularQuery = usePopularMoviesQuery();
  const searchQuery = useSearchMoviesQuery(props.debouncedTerm);

  const {
    data,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
    status,
  } = props.debouncedTerm ? searchQuery : popularQuery;

  useInfiniteScroll(
    loadMoreRef,
    fetchNextPage,
    Boolean(hasNextPage),
    isFetchingNextPage
  );

  // TBD - handle better duplicated movies
  const allMovies = data?.pages.flatMap((page) => page.results) ?? [];
  const movies = Array.from(
    new Map(allMovies.map((movie) => [movie.id, movie])).values()
  );

  return (
    <>
      {data ? (
        <Grid
          items={movies}
          keyExtractor={(movie) => String(movie.id)}
          renderItem={(movie) => <MovieCard movieData={movie} />}
        />
      ) : (
        <GridStatusMessage
          isPending={
            status === "pending" || (isFetching && !isFetchingNextPage)
          }
          isError={status === "error"}
          isSuccess={movies.length > 0}
          pendingText="Loading movies…"
          errorText="There was an error loading movies."
          isNoResults={!hasNextPage}
          noResultsText="No more movies to laod"
        />
      )}
      <div ref={loadMoreRef} style={{ height: 1 }} />

      <GridFooterMessageWrapper>
        {hasNextPage ? (
          <Text>Loading more…</Text>
        ) : (
          <Text>No more movies to load</Text>
        )}
      </GridFooterMessageWrapper>
    </>
  );
};
