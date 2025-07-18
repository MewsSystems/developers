import React, { memo } from "react";
import MovieCard from "../MovieCard/MovieCard";
import { StyledMovieList } from "./MovieList.styles";
import InfiniteScroll from "react-infinite-scroll-component";
import Loader from "../Loader/Loader";

import type { MovieType } from "../../types";

const MovieList = ({
  movies,
  hasMore,
  onFetchMoreMoviesRequest,
}: {
  movies: MovieType[];
  hasMore: boolean;
  onFetchMoreMoviesRequest: () => void;
}) => {
  return (
    <InfiniteScroll
      dataLength={movies.length}
      next={onFetchMoreMoviesRequest}
      hasMore={hasMore}
      loader={<Loader />}
    >
      <StyledMovieList>
        {movies.map((movie, i) => (
          <MovieCard key={i} movie={movie} />
        ))}
      </StyledMovieList>
    </InfiniteScroll>
  );
};

export default memo(MovieList);
