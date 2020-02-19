import { moviesActionTypes } from './MoviesConstants';

export function changeGridSettingsRequest(options, syncColumnsSettingsRequest) {
  return {
    type: moviesActionTypes.CHANGE_CARRIERS_GRID_SETTINGS_REQUEST,
    payload: {
      options,
      syncColumnsSettingsRequest,
    },
  };
}

export function changeGridSettingsSuccess(response) {
  return {
    type: moviesActionTypes.CHANGE_CARRIERS_GRID_SETTINGS_SUCCESS,
    payload: {
      response,
    },
  };
}

export function getCarriersError(error) {
  return {
    type: moviesActionTypes.GET_CARRIERS_ERROR,
    payload: {
      error,
    },
  };
}

export function fetchMovies(query) {
  return {
    type: moviesActionTypes.FETCH_MOVIES,
    payload: {
      query,
    },
  };
}

export function fetchMoviesSuccess(movies) {
  return {
    type: moviesActionTypes.FETCH_MOVIES_SUCCESS,
    payload: {
      movies,
    },
  };
}
