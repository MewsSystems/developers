import { AppReduxState } from "./state";
import { MoviesPage } from "../types";

const getLoading = (state: AppReduxState): boolean => state.loading;

const getCurrentPage = (state: AppReduxState): number => state.currentPage;

const getSearchMovieTitle = (state: AppReduxState): string =>
  state.searchMovieTitle;

const shouldFetchMovies = (state: AppReduxState): boolean => {
  const { searchMovieTitle, currentPage } = state;
  return !state.moviePages.find(
    (page) =>
      page.searchValue === searchMovieTitle && page.pageNumber === currentPage
  );
};

const getMoviePage = (state: AppReduxState): MoviesPage => {
  const { searchMovieTitle, currentPage } = state;
  const page = state.moviePages.find(
    (page) =>
      page.searchValue === searchMovieTitle && page.pageNumber === currentPage
  );
  return page || { totalPages: 0, movies: [], pageNumber: 1, searchValue: "" };
};

export const appSelectors = {
  getLoading,
  getCurrentPage,
  getSearchMovieTitle,
  shouldFetchMovies,
  getMoviePage,
};
