import { useEffect, useState, useCallback, useMemo } from "react";
import { ImdbMovieResponse, Movie } from "../../../models/tmdbModels";
import { MoviePosterCard } from "../../atoms/movie/movie-poster-card";
import { tmdbService } from "../../../services/tmdbServie";
import styled from "styled-components";

export const MovieSearcherResult: React.FC<{ query: string }> = ({ query }) => {
  const [imdbResult, setImdbResult] = useState<ImdbMovieResponse<Movie[]>>();
  const [movies, setMovies] = useState<Movie[]>([]);
  const [page, setPage] = useState<number>(1);
  const [errorMessage, setErrorMessage] = useState<null | string>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const fetchMovies = useCallback(
    async (currentPage: number) => {
      const queryToSend = query.replaceAll(" ", "%20");
      try {
        setIsLoading(true);
        const response = await tmdbService.get<ImdbMovieResponse<Movie[]>>(
          `/3/search/movie?query=${queryToSend}&language=en-US&page=${currentPage}`
        );

        setErrorMessage("");
        setImdbResult(response);
        if (response.results.length > 0) {
          setMovies((prev) => [...prev, ...response.results]);
        }
      } catch (error) {
        console.log(error);
        setErrorMessage("Failed to fetch movies. Please try again.");
      } finally {
        setIsLoading(false);
      }
    },
    [query]
  );

  const handleScroll = useCallback(() => {
    if (
      window.innerHeight + window.scrollY >=
        document.documentElement.scrollHeight - 100 &&
      !isLoading
    ) {
      setPage((prevPage) => prevPage + 1);
    }
  }, [isLoading]);

  useEffect(() => {
    setPage(1);
    setMovies([]);
    setErrorMessage(null);
    fetchMovies(1);
  }, [query, fetchMovies]);

  useEffect(() => {
    if (page > 1) {
      fetchMovies(page);
    }
  }, [page, fetchMovies]);

  useEffect(() => {
    if (movies.length > 0) {
      window.addEventListener("scroll", handleScroll);
      return () => {
        window.removeEventListener("scroll", handleScroll);
      };
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [handleScroll]);

  const renderedMovies = useMemo(
    () =>
      movies.map((movie, index) => (
        <MovieCardWrapper key={index}>
          <MoviePosterCard movie={movie} info />
        </MovieCardWrapper>
      )),
    [movies]
  );

  return (
    <Container>
      {errorMessage && errorMessage.length > 0 ? (
        <ErrorMessage>{errorMessage}</ErrorMessage>
      ) : (
        <>
          {isLoading ? (
            <LoadingMessage>Loading...</LoadingMessage>
          ) : (
            <ResultHeader>
              Results for {query}... (
              {imdbResult?.total_results && imdbResult?.total_results >= 1000
                ? "more than 1000"
                : imdbResult?.total_results}{" "}
              total movies)
            </ResultHeader>
          )}

          <MoviesGrid>{renderedMovies}</MoviesGrid>

          {errorMessage && <ErrorText>{errorMessage}</ErrorText>}
        </>
      )}
    </Container>
  );
};

// Styled Components
const Container = styled.div`
  margin: 32px 16px;
  font-size: 30px;
  font-weight: 600;
`;

const ResultHeader = styled.h1`
  color: #c4ab9c;
`;

const LoadingMessage = styled.h1`
  color: #c4ab9c;
`;

const ErrorMessage = styled.h1`
  color: #c4ab9c;
`;

const MoviesGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
  margin-top: 16px;
  @media (min-width: 640px) {
    grid-template-columns: repeat(4, 1fr);
  }
  @media (min-width: 768px) {
    grid-template-columns: repeat(5, 1fr);
  }
  @media (min-width: 1024px) {
    grid-template-columns: repeat(6, 1fr);
  }
`;

const MovieCardWrapper = styled.div`
  display: flex;
  justify-content: center;
`;

const ErrorText = styled.p`
  text-align: center;
  margin-top: 16px;
  color: #f87171;
`;
