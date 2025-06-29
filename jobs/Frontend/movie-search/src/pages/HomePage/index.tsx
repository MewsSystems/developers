import { useEffect, useRef, useState } from "react";
import { Input } from "../../components/Form/Input";
import { MainContent } from "../../components/Layout/MainContent";
import { Header } from "../../components/Header/Header";
import { Grid } from "../../components/Grid/Grid";
import { type Movie } from "../../api/types";
import { MovieCard } from "./MovieCard";
import { Heading } from "../../components/Typography/Heading";
import { usePopularMoviesQuery } from "../../hooks/useMoviesQueries";

export const HomePage = () => {
  const [movieData, setMovieData] = useState<Movie[]>([]);

  const {
    data,
    error,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isFetchingNextPage,
    status,
  } = usePopularMoviesQuery();

  const movies: Movie[] = data?.pages.flatMap((page) => page.results) ?? [];

  const loadMoreRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!hasNextPage || isFetchingNextPage) return;
    const observer = new IntersectionObserver(([entry]) => {
      if (entry.isIntersecting) {
        fetchNextPage();
      }
    });
    const el = loadMoreRef.current;
    if (el) observer.observe(el);
    return () => observer.disconnect();
  }, [hasNextPage, isFetchingNextPage, fetchNextPage]);

  if (status === "pending") {
    return <p>Loading data...</p>;
  }

  if (status === "error") {
    console.error(error);
    return <p>There was an error loading movies.</p>;
  }

  return (
    <>
      <Header>
        <Input name="searchField" />
      </Header>
      <MainContent>
        <Heading>List of movies</Heading>
        {movies ? (
          <>
            <Grid
              items={movies}
              keyExtractor={(movie) => String(movie.id)}
              renderItem={(movie) => <MovieCard movieData={movie} />}
            />

            {/* sentinel div triggers fetchNextPage when visible */}
            <div ref={loadMoreRef} style={{ height: 1 }} />

            {isFetchingNextPage && <p>Loading more…</p>}
            {!hasNextPage && <p>No more movies.</p>}
          </>
        ) : (
          <p>No data present</p>
        )}
      </MainContent>
    </>
  );
};
