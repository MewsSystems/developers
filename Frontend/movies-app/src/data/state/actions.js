import Actions from './actionTypes';

const LoadMovies = (searchTerm, page = 1) => ({
  type: Actions.LOAD_MOVIES,
  page: page,
  searchTerm
});
const LoadMoviesPage = page => ({
  type: Actions.LOAD_MOVIES_PAGE,
  page: page
});

const MoviesLoaded = data => ({
  type: Actions.LOAD_MOVIES_SUCCESS,
  data
});

const MoviesError = error => ({
  type: Actions.LOAD_MOVIES_ERROR,
  error
});

const LoadDetail = id => ({
  type: Actions.LOAD_DETAIL,
  id
});

const DetailLoaded = data => ({
  type: Actions.LOAD_DETAIL_SUCCESS,
  data
});

const DetailError = error => ({
  type: Actions.LOAD_DETAIL_ERROR,
  error
});

export { LoadMovies, LoadMoviesPage, MoviesLoaded, MoviesError, LoadDetail, DetailLoaded, DetailError };
