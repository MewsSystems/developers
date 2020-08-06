import constants from '../constants';
import { MovieDetail, Action } from '../types';

const reducer = (state: MovieDetail | {} = {}, action: Action) => {
  switch (action.type) {
    case constants.FETCH_MOVIE_ERROR: {
      alert('error occured');
      return state;
    }

    case constants.FETCH_MOVIE_SUCCESS: {
      return action.payload;
    }

    case constants.RESET_MOVIE: {
      return {};
    }

    default: {
      return state;
    }
  }
};

export default reducer;
