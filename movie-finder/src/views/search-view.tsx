import React, { useEffect, useMemo, useState } from "react";
import axios from "axios";
import { MovieTile } from "../components/movie-tile";
import { debounce } from "lodash";
import styled from "styled-components";
import { API_KEY } from "../constants";
import { InputText } from "../components/input";
import { Movie } from "../types";

interface ISearchViewProps {
  children?: never;
}

export const SearchView: React.FC<ISearchViewProps> = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [movieTitle, setMovieTitle] = useState("");

  const debouncedFetchMoviesByTitle = useMemo(
    () =>
      debounce((movieTitle: string) => {
        axios
          .get(
            `https://api.themoviedb.org/3/search/movie?api_key=${API_KEY}&language=en-US&query=${movieTitle}&page=1&include_adult=false`
          )
          .then((response) => setMovies(response.data.results));
      }, 1500),
    []
  );

  useEffect(() => {
    if (movieTitle) {
      debouncedFetchMoviesByTitle(movieTitle);
    }
  }, [movieTitle, debouncedFetchMoviesByTitle]);

  return (
    <SearchViewLayout>
      <InputContainer>
        <InputText
          value={movieTitle}
          placeholder={"Find your favourite movie"}
          onChange={(event) => setMovieTitle(event.target.value)}
        />
      </InputContainer>
      <MoviesContainer>
        {movies.map((movie) => {
          const {
            id,
            original_title,
            overview,
            vote_average,
            vote_count,
            poster_path,
          } = movie;
          return (
            <MovieTile
              key={id}
              movieId={id}
              originalTitle={original_title}
              overview={overview}
              voteAverage={vote_average}
              voteCount={vote_count}
              posterSrc={poster_path}
            />
          );
        })}
      </MoviesContainer>
    </SearchViewLayout>
  );
};

const TOP_VIEW_OFFSET = "4rem";

const SearchViewLayout = styled.div`
  width: 100%;
  height: 100%;
  display: grid;
  gap: 1rem;
  grid-template-columns: 1fr 1fr;
  grid-template-rows: 2rem 1fr 2rem;
  grid-template-areas:
    "search search"
    "movies movies"
    "pagination pagination";
  padding: ${TOP_VIEW_OFFSET} 0 0 0;
  background: ${(props) => props.theme.color.grey};
`;

const MoviesContainer = styled.div`
  grid-area: movies;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  min-height: 100vw;
  margin-top: ${TOP_VIEW_OFFSET};
  & > div {
    margin: 1rem;
    max-width: 45%;
    min-width: 30rem;
  }
`;

const InputContainer = styled.div`
  grid-area: search;
  display: flex;
  width: 100%;
  justify-content: center;

  & > input {
    width: 30rem;
  }
`;
