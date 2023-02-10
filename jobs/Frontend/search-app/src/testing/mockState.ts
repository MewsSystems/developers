import { MoviesListState } from "../public-api";

export const mockState: MoviesListState = {
  searchKey: "abc",
  movies: [],
  isBusy: false,
  error: null,
  activePage: 1,
  totalPages: 20,
  results: 1000,
  movieDetail: null,
  movieDetailId: "",
};
