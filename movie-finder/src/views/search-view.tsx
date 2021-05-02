import React, { useEffect, useMemo, useState } from "react";
import axios from "axios";
import { MovieTile } from "../components/movie-tile";
import { debounce } from "lodash";
import styled from "styled-components";
import { API_KEY } from "../constants";

type Movie = {
  adult: boolean;
  backdrop_path: string;
  id: number;
  original_title: string;
  overview: string;
  poster_path: string;
  release_date: string;
  title: string;
  vote_average: number;
  vote_count: number;
};

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
      <input
        value={movieTitle}
        onChange={(event) => setMovieTitle(event.target.value)}
      />
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
  padding: 0 1rem;
  background: ${(props) => props.theme.color.grey};
`;

const MoviesContainer = styled.div`
  grid-area: movies;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  & > div {
    margin: 1rem;
    max-width: 45%;
    min-width: 30rem;
  }
`;
