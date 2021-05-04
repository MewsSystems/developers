import {
  ActionTypes,
  SetCurrentPageAction,
  SetLoadingAction,
  SetMoviePageAction,
  SetPageToSearchAction,
  SetSearchMovieTitleAction,
  SetTotalPagesAction,
} from "./types";
import { MoviesPage } from "../types";

const setLoading = (loading: boolean): SetLoadingAction => ({
  type: ActionTypes.SET_LOADING,
  payload: loading,
});

const setSearchMovieTitle = (title: string): SetSearchMovieTitleAction => ({
  type: ActionTypes.SET_SEARCH_MOVIE_TITLE,
  payload: title,
});

const setCurrentPage = (currentPage: number): SetCurrentPageAction => ({
  type: ActionTypes.SET_CURRENT_PAGE,
  payload: currentPage,
});

const setPageToSearch = (pageToSearch: number): SetPageToSearchAction => ({
  type: ActionTypes.SET_PAGE_TO_SEARCH,
  payload: pageToSearch,
});

const setTotalPages = (totalPages: number): SetTotalPagesAction => ({
  type: ActionTypes.SET_TOTAL_PAGES,
  payload: totalPages,
});

const setMoviePage = (moviePage: MoviesPage): SetMoviePageAction => ({
  type: ActionTypes.SET_MOVIE_PAGE,
  payload: moviePage,
});

export const appActionCreators = {
  setLoading,
  setSearchMovieTitle,
  setCurrentPage,
  setPageToSearch,
  setTotalPages,
  setMoviePage,
};
