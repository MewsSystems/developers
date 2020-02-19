import { movieDetailsActionTypes } from './MovieDetailsConstants';

const initialState = {
  loading: false,
  movieDetails: {
    results: []
  },
};

export default function movieDetailsReducer(state = initialState, action) {
  const { type, payload = {} } = action;

  switch(type) {
    case movieDetailsActionTypes.FETCH_MOVIE_DETAILS:
      return {
        ...state,
        loading: true,
      };

    case movieDetailsActionTypes.FETCH_MOVIE_DETAILS_SUCCESS:
      const { movie } = payload || {};

      return {
        ...state,
        movie: movie,
        loading: false,
      };

    case movieDetailsActionTypes.FETCH_MOVIE_DETAILS_ERROR:
      return {
        ...state,
        loading: false,
      };

    default:
      return state;
  }
}
