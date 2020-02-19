import keyMirror from 'keymirror';

export const moviesActionTypes = keyMirror(
  {
    FETCH_MOVIES: null,
    FETCH_MOVIES_SUCCESS: null,
    FETCH_MOVIES_ERROR: null,
  }
);
