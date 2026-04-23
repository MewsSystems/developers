import { Movie } from '../types';

export const UPDATE_RESULTS = 'UPDATE_RESULTS';

const initialState = {};

const resultsReducer = (
  state = initialState,
  action: { type: string; payload: { results?: Movie[] } },
) => {
  switch (action.type) {
    case UPDATE_RESULTS:
      return { ...state, ...action.payload.results };
    default:
      return state;
  }
};

export default resultsReducer;
