import React, { useEffect } from "react";
import { HomeDiscover } from "../organisms/search/discover";
import { MainTopicsMoviesCarrousels } from "../organisms/movie/main-topics-movies-carrousels";
import { useAppDispatch, useAppSelector } from "../../redux/reduxHooks";
import { MovieSearcherResult } from "../molecules/search/movie-searcher-result";
import { setMovieSearched } from "../../redux/searchEngine";

export const HomePage: React.FC = () => {
  const dispatch = useAppDispatch();
  const currentMovieSearch = useAppSelector(
    (state) => state.searchEngine.movieSearched
  );

  useEffect(() => {
    dispatch(setMovieSearched(""));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  return (
    <>
      <HomeDiscover />
      {currentMovieSearch.length > 0 ? (
        <MovieSearcherResult query={currentMovieSearch} />
      ) : (
        <MainTopicsMoviesCarrousels />
      )}
    </>
  );
};
