import { useRef } from "react";
import { Grid } from "../../components/Grid/Grid";
import { type Movie } from "../../api/types";
import { MovieCard } from "./MovieCard";
import { usePopularMoviesQuery } from "../../hooks/useMoviesQueries";
import { useSearchMoviesQuery } from "../../hooks/useSearchMoviesQuery";
import { useInfiniteScroll } from "../../hooks/useInfiniteScroll";

interface MoviesListProps {
  debouncedTerm: string;
}

export const MoviesList = (props: MoviesListProps) => {
  const loadMoreRef = useRef<HTMLDivElement | null>(null);

  const popularQuery = usePopularMoviesQuery();
  const searchQuery = useSearchMoviesQuery(props.debouncedTerm);

  const {
    data,
    error,
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

  const movies: Movie[] = data?.pages.flatMap((page) => page.results) ?? [];

  if (status === "pending") {
    return <p>Loading data…</p>;
  }

  if (status === "error") {
    console.error(error);
    return <p>There was an error loading movies.</p>;
  }

  return (
    <>
      {movies ? (
        <>
          <Grid
            items={movies}
            keyExtractor={(movie) => String(movie.id)}
            renderItem={(movie) => <MovieCard movieData={movie} />}
          />
          <div ref={loadMoreRef} style={{ height: 1 }} />

          {isFetchingNextPage && <p>Loading more…</p>}
          {!hasNextPage && <p>No more movies.</p>}
        </>
      ) : (
        <p>No data present</p>
      )}
    </>
  );
};
