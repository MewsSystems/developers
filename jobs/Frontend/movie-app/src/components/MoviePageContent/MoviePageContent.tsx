import React, { useCallback, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../../state/store";
import {
  fetchPopularMovies,
  searchMovies,
} from "../../state/movies/moviesSlice";
import MovieList from "../MovieList/MovieList";
import EmptyState from "../EmptyState/EmptyState";

const MoviePageContent = () => {
  const dispatch = useDispatch<AppDispatch>();
  const {
    popularMovies,
    popularMoviesPage,
    popularMoviesTotalPages,
    searchedMovies,
    searchedMoviesPage,
    searchedMoviesTotalPages,
    searchQuery,
    searchedMoviesStatus,
  } = useSelector((state: RootState) => state.movies);

  useEffect(() => {
    dispatch(fetchPopularMovies({ page: 1 }));
  }, [dispatch]);

  const handleFetchMorePopularMoviesRequest = useCallback(() => {
    dispatch(fetchPopularMovies({ page: popularMoviesPage + 1 }));
  }, [popularMoviesPage, dispatch]);

  const handleFetchMoreSearchMoviesRequest = useCallback(() => {
    dispatch(searchMovies({ searchQuery, page: searchedMoviesPage + 1 }));
  }, [searchQuery, searchedMoviesPage, dispatch]);

  if (
    searchQuery.length &&
    searchedMovies.length === 0 &&
    searchedMoviesStatus === "succeeded"
  ) {
    return <EmptyState />;
  }

  if (searchQuery.length && searchedMoviesStatus === "succeeded") {
    return (
      <MovieList
        movies={searchedMovies}
        hasMore={searchedMoviesPage !== searchedMoviesTotalPages}
        onFetchMoreMoviesRequest={handleFetchMoreSearchMoviesRequest}
      />
    );
  }

  if (popularMovies.length) {
    return (
      <MovieList
        movies={popularMovies}
        hasMore={popularMoviesPage !== popularMoviesTotalPages}
        onFetchMoreMoviesRequest={handleFetchMorePopularMoviesRequest}
      />
    );
  }

  return null;
};

export default MoviePageContent;
