import { Movie } from '../types';
import { TRIGGER_SEARCH } from './searchReducer';

export const SET_SELECTED_RESULT = 'SET_SELECTED_RESULT';

const initialState = {};

const detailReducer = (
  state = initialState,
  action: { type: string; payload: { result?: Movie } },
) => {
  switch (action.type) {
    case SET_SELECTED_RESULT:
      return { ...action.payload.result };
    case TRIGGER_SEARCH:
      return initialState;
    default:
      return state;
  }
};

export default detailReducer;
