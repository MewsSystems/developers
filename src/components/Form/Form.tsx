import React, { useState } from "react";
import styled from "styled-components";
import { MovieItem } from "../MovieItem/MovieItem";
import { MovieDetails } from "../MovieDetails/MovieDetails";
import { ScrollToTopButton } from "../ScrollToTopButton/ScrollToTopButton";
import { useMovies } from "../../hooks/useMovies/useMovies";
import { useScroll } from "../../hooks/useScroll/useScroll";
import { useGenres } from "../../hooks/useGenres/useGenres";

export type QueryType=string;
export type PageType=number;

export interface Movie {
  id: number;
  title: string;
  poster_path: string;
  overview: string;
  genre_ids: number[];
  release_date: string;
  vote_average: number;
  vote_count: number;
}

const StyledFormLabel = styled.label`
  color: black;
  display: flex;
  align-items: center;
  justify-content: start;
  font-size: calc(10px + 2vmin);
  margin: calc(10px + 2vmin);
`;

const Input = styled.input`
  position: sticky;
  width: 60%;
  height: calc(10px + 2vmin);
  font-size: calc(10px + 1vmin);
  &:not(:focus) {
    box-shadow: 0 0 2px 2px #ff6a00;
  }
`;

const MovieList = styled.ul`
  list-style: none;
  padding: 0;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
`;

const LoadMoreButton = styled.button`
  padding: 2vmin 3vmin;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 25px;
  cursor: pointer;
  margin-bottom: 5vmin;
  font-size: calc(10px + 2vmin);

  &:hover {
    background-color: #ff6a00;
  }
`;

export const Form:React.FC = () => {

  const [query, setQuery] = useState<QueryType>("");
  const [page, setPage] = useState<PageType>(1);
  const [movieSelected, setMovieSelected] = useState<Movie|null>(null);
  const { movies, hasMore, loading, error } = useMovies(query, page);
  const { genres } = useGenres();
  const showScrollButton = useScroll();

  const handleInputChange = (e:React.ChangeEvent<HTMLInputElement>) => {
    setQuery(e.target.value);
    setPage(1);
  };

  const handleLoadMore = () => {
    setPage((prevPage) => prevPage + 1);
  };

  const scrollToTop = () => {
    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  };

  const handleMovieSelected = (movie:Movie) => {
    setMovieSelected(movie);
  };

  const handleBackToList = () => {
    setMovieSelected(null);
  };

  return (
    <>
      {!movieSelected && (
        <form onSubmit={(e) => e.preventDefault()}>
          <StyledFormLabel htmlFor="movie-input">
            What would you like to find?
          </StyledFormLabel>
          <Input
            id="movie-input"
            type="text"
            placeholder="Type your search here"
            value={query}
            onChange={handleInputChange}
          />
        </form>
      )}
      {loading && <p>Loading...</p>}
      {error && <p>{error}</p>}
      {movieSelected ? (
        <MovieDetails
          movie={movieSelected}
          onBackToList={handleBackToList}
          genres={genres}
        />
      ) : (
        <>
          <MovieList>
            {movies.map((movie:Movie) => (
              <MovieItem
                key={movie.id}
                movie={movie}
                onClick={() => handleMovieSelected(movie)}
              />
            ))}
          </MovieList>
          {hasMore && (
            <LoadMoreButton onClick={handleLoadMore}>Load More</LoadMoreButton>
          )}
        </>
      )}

      <ScrollToTopButton
        onClick={scrollToTop}
        show={showScrollButton}
      />
    </>
  );
};
