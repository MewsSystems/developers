import { useRef, useCallback } from "react";
import { useSearchParams } from "react-router-dom";

import SearchInput from "../../components/SearchInput";
import Card from "../../components/Card";
import { getPosterPath, getPlaceholderPosterPath } from "../../api";
import { useMoviesQuery } from "../../hooks/useMoviesQuery";
import Spinner from "../../components/Spinner";
import Error from "../../components/Error";
import {
  HomeContainer,
  MovieContainer,
  MovieCardContainer,
} from "./Home.styles";

type Movie = {
  id: number;
  poster_path: string;
  title: string;
};

function Home() {
  const {
    movies,
    error,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isLoading,
    isError,
  } = useMoviesQuery();

  const [searchParams, setSearchParams] = useSearchParams();
  const query = searchParams.get("query");

  const observer = useRef<IntersectionObserver>();
  const lastElement = useCallback(
    (node: HTMLDivElement) => {
      if (isLoading) return;
      if (observer.current) observer.current.disconnect();
      observer.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting && hasNextPage && !isFetching) {
          fetchNextPage();
        }
      });
      if (node) observer.current.observe(node);
    },
    [isLoading, hasNextPage, fetchNextPage, isFetching]
  );

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchParams(
      (prev) => {
        !e.target.value
          ? prev.delete("query")
          : prev.set("query", e.target.value);

        return prev;
      },
      { replace: true }
    );
  };

  return (
    <HomeContainer>
      <SearchInput
        placeholder="Search your movie"
        handleChange={handleChange}
        query={query}
      />
      <MovieContainer>
        {isLoading ? (
          <Spinner />
        ) : isError ? (
          <Error errorMessage={error?.message} />
        ) : (
          <>
            <MovieCardContainer data-testid="movie-card-container">
              {movies?.map((movie: Movie) => (
                <Card
                  observerRef={lastElement}
                  // TODO: API sometimes returns duplicate results!
                  // Filter out results at the API layer.
                  key={movie.id}
                  cardDetailLink={`/movies/${movie.id}`}
                  cardImage={
                    movie.poster_path
                      ? getPosterPath(movie.poster_path)
                      : getPlaceholderPosterPath(movie.title)
                  }
                  altText={movie.title}
                />
              ))}
            </MovieCardContainer>
          </>
        )}
      </MovieContainer>
    </HomeContainer>
  );
}

export default Home;
