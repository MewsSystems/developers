import keyMirror from 'keymirror';

export const moviesActionTypes = keyMirror(
  {
    FETCH_MOVIES: null,
    FETCH_MOVIES_SUCCESS: null,
    FETCH_MOVIES_ERROR: null,


    GET_CARRIERS_ERROR: null,
    CHANGE_CARRIERS_GRID_SETTINGS_REQUEST: null,
    CHANGE_CARRIERS_GRID_SETTINGS_SUCCESS: null,
  }
);
